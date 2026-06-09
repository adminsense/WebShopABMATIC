using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Profile;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Audit;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Store;

public sealed class StoreProfileRepository : IStoreProfileRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly WebShopABMATICDbContext _db;
    private readonly IAuditService _audit;

    public StoreProfileRepository(
        UserManager<ApplicationUser> userManager,
        WebShopABMATICDbContext db,
        IAuditService audit)
    {
        _userManager = userManager;
        _db = db;
        _audit = audit;
    }

    public async Task<StoreProfileDto?> GetAsync(string identityUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(identityUserId);
        if (user is null)
        {
            return null;
        }

        var customer = await _db.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdentityUserId == identityUserId, cancellationToken);

        if (customer is null)
        {
            return new StoreProfileDto
            {
                Email = user.Email ?? user.UserName ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber ?? string.Empty
            };
        }

        var (postalCode, cityName) = await WebshopAddressHelper.GetCityDisplayAsync(
            _db, customer.CustomerCityId, cancellationToken);

        return new StoreProfileDto
        {
            CustomerId = customer.CustomerId,
            Email = user.Email ?? user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = string.IsNullOrWhiteSpace(user.PhoneNumber) ? customer.CustomerPhone : user.PhoneNumber,
            Street = customer.CustomerStreet,
            HouseNumber = customer.CustomerHouseNumber,
            Box = customer.CustomerBox,
            PostalCode = postalCode,
            CityName = cityName
        };
    }

    public async Task<StoreProfileSaveResult> SaveAsync(
        string identityUserId,
        StoreProfileUpdateDto profile,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(identityUserId);
        if (user is null)
        {
            return Fail("Account not found.");
        }

        user.FirstName = profile.FirstName.Trim();
        user.LastName = profile.LastName.Trim();
        user.PhoneNumber = profile.Phone?.Trim();

        var userResult = await _userManager.UpdateAsync(user);
        if (!userResult.Succeeded)
        {
            return Fail(userResult.Errors.Select(e => e.Description).ToArray());
        }

        var customer = await _db.Customers
            .FirstOrDefaultAsync(c => c.IdentityUserId == identityUserId, cancellationToken);

        if (customer is null)
        {
            await AuditManualLogger.LogIdentityUserAsync(
                _audit,
                AuditActions.Update,
                user.Id,
                user.Email,
                new
                {
                    profile = "store",
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    phone = user.PhoneNumber
                },
                cancellationToken);
            return new StoreProfileSaveResult { Succeeded = true };
        }

        var cityId = await WebshopAddressHelper.ResolveCityIdAsync(
            _db, profile.PostalCode, profile.CityName, cancellationToken);

        customer.CustomerName = $"{user.FirstName} {user.LastName}".Trim();
        customer.FirstContactName = customer.CustomerName;
        customer.CustomerPhone = profile.Phone?.Trim() ?? string.Empty;
        customer.CustomerStreet = profile.Street.Trim();
        customer.CustomerHouseNumber = profile.HouseNumber.Trim();
        customer.CustomerBox = profile.Box?.Trim() ?? string.Empty;
        customer.CustomerCityId = cityId;

        await _db.SaveChangesAsync(cancellationToken);
        await SyncDefaultDeliveryAddressAsync(customer, cityId, cancellationToken);

        await AuditManualLogger.LogIdentityUserAsync(
            _audit,
            AuditActions.Update,
            user.Id,
            user.Email,
            new
            {
                profile = "store",
                firstName = user.FirstName,
                lastName = user.LastName,
                phone = user.PhoneNumber,
                customerId = customer.CustomerId
            },
            cancellationToken);

        return new StoreProfileSaveResult { Succeeded = true };
    }

    private async Task SyncDefaultDeliveryAddressAsync(Customer customer, int cityId, CancellationToken cancellationToken)
    {
        var address = await _db.CustomerDeliveryAddresses
            .FirstOrDefaultAsync(a => a.CustomerId == customer.CustomerId, cancellationToken);

        if (address is null)
        {
            _db.CustomerDeliveryAddresses.Add(new CustomerDeliveryAddress
            {
                CustomerId = customer.CustomerId,
                Name = customer.CustomerName,
                Straat = customer.CustomerStreet,
                Number = customer.CustomerHouseNumber,
                Bus = customer.CustomerBox,
                CityId = cityId,
                Notes = string.Empty
            });
        }
        else
        {
            address.Name = customer.CustomerName;
            address.Straat = customer.CustomerStreet;
            address.Number = customer.CustomerHouseNumber;
            address.Bus = customer.CustomerBox;
            address.CityId = cityId;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static StoreProfileSaveResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
