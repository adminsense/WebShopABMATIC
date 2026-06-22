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
            db.Customers.Add(customer);
        }

        await db.SaveChangesAsync(cancellationToken);
        await EnsureDefaultDeliveryAddressAsync(customer, cityId, cancellationToken);
        await EnsureProjectAsync(customer, fullName, cancellationToken);

        return new CustomerRegistrationResult
        {
            Succeeded = true,
            CustomerId = customer.CustomerId
        };
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

    private async Task EnsureProjectAsync(Customer customer, string customerName, CancellationToken cancellationToken)
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
            ProjectManagerUserId = 1,
            CustomerId = customer.CustomerId,
            ProjectTypeId = 1,
            IsStandardProject = true
        });

        await db.SaveChangesAsync(cancellationToken);
    }

    private static CustomerRegistrationResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
