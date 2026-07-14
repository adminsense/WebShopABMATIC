using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Registration;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Infrastructure.Store;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class CustomerRegistrationRepository(WebShopABMATICDbContext db) : ICustomerRegistrationRepository
{
    public async Task<CustomerRegistrationResult> RegisterAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim();
        var normalized = email.ToLowerInvariant();

        var existing = await db.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c =>
                (c.WebshopLogin != null && c.WebshopLogin.ToLower() == normalized) ||
                c.CustomerEmail.ToLower() == normalized, cancellationToken);

        if (existing is not null && !string.IsNullOrWhiteSpace(existing.WebshopPasswordHash))
        {
            return Fail("An account with this email already exists.");
        }

        var fullName = $"{request.FirstName.Trim()} {request.LastName.Trim()}".Trim();
        var cityId = await WebshopAddressHelper.ResolveCityIdAsync(
            db, request.PostalCode, request.CityName, cancellationToken);
        var (hash, salt) = LegacyWebshopPasswordVerifier.CreateHash(request.Password);

        RegistrationDefaults defaults;
        try
        {
            defaults = await ResolveDefaultsAsync(cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return Fail(ex.Message);
        }

        try
        {
            Customer customer;
            if (existing is not null)
            {
                customer = await db.Customers.FirstAsync(c => c.CustomerId == existing.CustomerId, cancellationToken);
                customer.WebshopLogin ??= email;
                customer.WebshopPasswordHash = hash;
                customer.WebshopPasswordSalt = salt;
                customer.CustomerName = fullName;
                customer.FirstContactName = fullName;
                customer.CustomerPhone = request.Phone.Trim();
                customer.CustomerStreet = request.Street.Trim();
                customer.CustomerHouseNumber = request.HouseNumber.Trim();
                customer.CustomerBox = request.Box?.Trim() ?? string.Empty;
                customer.CustomerCityId = cityId;
                if (string.IsNullOrWhiteSpace(customer.CustomerEmail))
                {
                    customer.CustomerEmail = email;
                }
            }
            else
            {
                customer = (Customer)AdminCrudDefaults.Create("customers");
                ApplyDefaults(customer, defaults);
                customer.CustomerName = fullName;
                customer.CustomerEmail = email;
                customer.WebshopLogin = email;
                customer.WebshopPasswordHash = hash;
                customer.WebshopPasswordSalt = salt;
                customer.FirstContactName = fullName;
                customer.CustomerPhone = request.Phone.Trim();
                customer.CustomerStreet = request.Street.Trim();
                customer.CustomerHouseNumber = request.HouseNumber.Trim();
                customer.CustomerBox = request.Box?.Trim() ?? string.Empty;
                customer.CustomerCityId = cityId;
                customer.CustomerVatNumber = string.Empty;
                customer.CustomerGroup = string.Empty;
                customer.CreatedAt = DateTime.UtcNow;
                customer.CreatedBy = "webshop";
                db.Customers.Add(customer);
            }

            await db.SaveChangesAsync(cancellationToken);
            await EnsureDefaultDeliveryAddressAsync(customer, cityId, cancellationToken);
            await EnsureProjectAsync(customer, fullName, defaults, cancellationToken);

            return new CustomerRegistrationResult
            {
                Succeeded = true,
                CustomerId = customer.CustomerId
            };
        }
        catch (DbUpdateException ex)
        {
            var detail = ex.InnerException?.Message ?? ex.Message;
            return Fail($"Could not create the account. ({detail})");
        }
    }

    private async Task<RegistrationDefaults> ResolveDefaultsAsync(CancellationToken cancellationToken)
    {
        var customerType = await db.CustomerTypes.AsNoTracking()
            .OrderByDescending(t => t.IsDefault == true)
            .ThenBy(t => t.SortOrder)
            .ThenBy(t => t.KlantTypeId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("No customer type is configured in the database.");

        var accountManagerUserId = await db.StaffUsers.AsNoTracking()
            .OrderBy(u => u.Id)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (accountManagerUserId == 0)
        {
            throw new InvalidOperationException("No staff user is available to assign as account manager.");
        }

        var statusId = await db.CustomerStatuses.AsNoTracking()
            .OrderByDescending(s => s.IsDefault == true)
            .ThenBy(s => s.Id)
            .Select(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (statusId == 0)
        {
            throw new InvalidOperationException("No customer status is configured in the database.");
        }

        var languageId = await db.Languages.AsNoTracking()
            .OrderBy(l => l.Id)
            .Select(l => l.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (languageId == 0)
        {
            throw new InvalidOperationException("No language is configured in the database.");
        }

        var paymentTermId = await ResolveExistingIdAsync(
            db.PaymentTerms.AsNoTracking().Where(p => p.Id == customerType.PaymentTermId).Select(p => p.Id),
            db.PaymentTerms.AsNoTracking().OrderBy(p => p.Id).Select(p => p.Id),
            cancellationToken,
            "payment term");

        var deliveryTypeId = await ResolveExistingIdAsync(
            db.DeliveryTypes.AsNoTracking().Where(d => d.Id == customerType.DeliveryTypeId).Select(d => d.Id),
            db.DeliveryTypes.AsNoTracking().OrderBy(d => d.Id).Select(d => d.Id),
            cancellationToken,
            "delivery type");

        var vatSystemId = await ResolveExistingIdAsync(
            db.VatTypes.AsNoTracking().Where(v => v.Id == customerType.VatSystemId).Select(v => v.Id),
            db.VatTypes.AsNoTracking().OrderBy(v => v.Id).Select(v => v.Id),
            cancellationToken,
            "VAT system");

        // No ProjectType lookup entity in this model — reuse a type already used by existing projects.
        var projectTypeId = await db.Projects.AsNoTracking()
            .Where(p => p.ProjectTypeId > 0)
            .OrderBy(p => p.ProjectTypeId)
            .Select(p => p.ProjectTypeId)
            .FirstOrDefaultAsync(cancellationToken);
        if (projectTypeId == 0)
        {
            throw new InvalidOperationException("No project type is configured in the database.");
        }

        return new RegistrationDefaults(
            CustomerTypeId: customerType.KlantTypeId,
            DeliveryCustomerTypeId: customerType.KlantTypeId,
            AccountManagerUserId: accountManagerUserId,
            CustomerStatusId: statusId,
            CustomerLanguageId: languageId,
            DeliveryTypeId: deliveryTypeId,
            BetaaltermijnId: paymentTermId,
            CustomerVatSystemId: vatSystemId,
            ProjectTypeId: projectTypeId);
    }

    private static async Task<int> ResolveExistingIdAsync(
        IQueryable<int> preferred,
        IQueryable<int> fallback,
        CancellationToken cancellationToken,
        string label)
    {
        var id = await preferred.FirstOrDefaultAsync(cancellationToken);
        if (id != 0)
        {
            return id;
        }

        id = await fallback.FirstOrDefaultAsync(cancellationToken);
        if (id != 0)
        {
            return id;
        }

        throw new InvalidOperationException($"No {label} is configured in the database.");
    }

    private static void ApplyDefaults(Customer customer, RegistrationDefaults defaults)
    {
        customer.CustomerTypeId = defaults.CustomerTypeId;
        customer.DeliveryCustomerTypeId = defaults.DeliveryCustomerTypeId;
        customer.AccountManagerUserId = defaults.AccountManagerUserId;
        customer.CustomerStatusId = defaults.CustomerStatusId;
        customer.CustomerLanguageId = defaults.CustomerLanguageId;
        customer.DeliveryTypeId = defaults.DeliveryTypeId;
        customer.BetaaltermijnId = defaults.BetaaltermijnId;
        customer.CustomerVatSystemId = defaults.CustomerVatSystemId;
        customer.BaseCompaniesId = 1;
    }

    private async Task EnsureDefaultDeliveryAddressAsync(Customer customer, int cityId, CancellationToken cancellationToken)
    {
        var exists = await db.CustomerDeliveryAddresses
            .AnyAsync(a => a.CustomerId == customer.CustomerId, cancellationToken);

        if (exists)
        {
            return;
        }

        db.CustomerDeliveryAddresses.Add(new CustomerDeliveryAddress
        {
            CustomerId = customer.CustomerId,
            Name = customer.CustomerName,
            Straat = customer.CustomerStreet,
            Number = customer.CustomerHouseNumber,
            Bus = customer.CustomerBox,
            CityId = cityId,
            Notes = string.Empty
        });

        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureProjectAsync(
        Customer customer,
        string customerName,
        RegistrationDefaults defaults,
        CancellationToken cancellationToken)
    {
        var hasProject = await db.Projects.AnyAsync(p => p.CustomerId == customer.CustomerId, cancellationToken);
        if (hasProject)
        {
            return;
        }

        var nextNumber = await db.Projects.MaxAsync(p => (int?)p.ProjectNumber, cancellationToken) ?? 1000;
        nextNumber++;

        db.Projects.Add(new Project
        {
            ProjectNumber = nextNumber,
            ProjectName = $"Webshop — {customerName}",
            ProjectManagerUserId = defaults.AccountManagerUserId,
            CustomerId = customer.CustomerId,
            ProjectTypeId = defaults.ProjectTypeId,
            IsStandardProject = true,
            ProjectCreatedAt = DateTime.UtcNow,
            BaseCompanyId = 1
        });

        await db.SaveChangesAsync(cancellationToken);
    }

    private static CustomerRegistrationResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };

    private sealed record RegistrationDefaults(
        int CustomerTypeId,
        int DeliveryCustomerTypeId,
        int AccountManagerUserId,
        int CustomerStatusId,
        int CustomerLanguageId,
        int DeliveryTypeId,
        int BetaaltermijnId,
        int CustomerVatSystemId,
        int ProjectTypeId);
}
