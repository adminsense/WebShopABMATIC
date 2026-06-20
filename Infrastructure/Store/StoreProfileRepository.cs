using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Profile;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Store;

public sealed class StoreProfileRepository(WebShopABMATICDbContext db) : IStoreProfileRepository
{
    public async Task<StoreProfileDto?> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await db.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);

        if (customer is null)
        {
            return null;
        }

        var (postalCode, cityName) = await WebshopAddressHelper.GetCityDisplayAsync(
            db, customer.CustomerCityId, cancellationToken);

        var nameParts = SplitName(customer.CustomerName);

        return new StoreProfileDto
        {
            CustomerId = customer.CustomerId,
            Email = customer.CustomerEmail,
            FirstName = nameParts.FirstName,
            LastName = nameParts.LastName,
            Phone = customer.CustomerPhone,
            Street = customer.CustomerStreet,
            HouseNumber = customer.CustomerHouseNumber,
            Box = customer.CustomerBox,
            PostalCode = postalCode,
            CityName = cityName
        };
    }

    public async Task<StoreProfileSaveResult> SaveByCustomerIdAsync(
        int customerId,
        StoreProfileUpdateDto profile,
        CancellationToken cancellationToken = default)
    {
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        if (customer is null)
        {
            return Fail("Customer not found.");
        }

        var cityId = await WebshopAddressHelper.ResolveCityIdAsync(
            db, profile.PostalCode, profile.CityName, cancellationToken);

        customer.CustomerName = $"{profile.FirstName.Trim()} {profile.LastName.Trim()}".Trim();
        customer.FirstContactName = customer.CustomerName;
        customer.CustomerPhone = profile.Phone?.Trim() ?? string.Empty;
        customer.CustomerStreet = profile.Street.Trim();
        customer.CustomerHouseNumber = profile.HouseNumber.Trim();
        customer.CustomerBox = profile.Box?.Trim() ?? string.Empty;
        customer.CustomerCityId = cityId;

        await db.SaveChangesAsync(cancellationToken);
        await SyncDefaultDeliveryAddressAsync(customer, cityId, cancellationToken);

        return new StoreProfileSaveResult { Succeeded = true };
    }

    private async Task SyncDefaultDeliveryAddressAsync(Customer customer, int cityId, CancellationToken cancellationToken)
    {
        var address = await db.CustomerDeliveryAddresses
            .FirstOrDefaultAsync(a => a.CustomerId == customer.CustomerId, cancellationToken);

        if (address is null)
        {
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
        }
        else
        {
            address.Name = customer.CustomerName;
            address.Straat = customer.CustomerStreet;
            address.Number = customer.CustomerHouseNumber;
            address.Bus = customer.CustomerBox;
            address.CityId = cityId;
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    private static StoreProfileSaveResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };

    private static (string FirstName, string LastName) SplitName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return ("", "");
        }

        var parts = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => ("", ""),
            1 => (parts[0], ""),
            _ => (parts[0], parts[1])
        };
    }
}
