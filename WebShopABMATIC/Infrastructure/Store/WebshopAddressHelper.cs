using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Store;

internal static class WebshopAddressHelper
{
    public static async Task<int> ResolveCityIdAsync(
        WebShopABMATICDbContext db,
        string postalCode,
        string cityName,
        CancellationToken cancellationToken)
    {
        var postal = postalCode.Trim();
        if (!string.IsNullOrEmpty(postal))
        {
            var byPostal = await db.Cities.AsNoTracking()
                .Where(c => c.PostalCode == postal)
                .Select(c => c.CityId)
                .FirstOrDefaultAsync(cancellationToken);

            if (byPostal != 0)
            {
                return byPostal;
            }
        }

        var city = cityName.Trim();
        if (!string.IsNullOrEmpty(city))
        {
            var byName = await db.Cities.AsNoTracking()
                .Where(c => c.CityName == city)
                .Select(c => c.CityId)
                .FirstOrDefaultAsync(cancellationToken);

            if (byName != 0)
            {
                return byName;
            }

            var created = new City
            {
                CityName = city,
                PostalCode = postal,
                CountryName = "Belgium",
                CountryIsoCode = "BE"
            };
            db.Cities.Add(created);
            await db.SaveChangesAsync(cancellationToken);
            return created.CityId;
        }

        return 1;
    }

    public static async Task<(string PostalCode, string CityName)> GetCityDisplayAsync(
        WebShopABMATICDbContext db,
        int cityId,
        CancellationToken cancellationToken)
    {
        var city = await db.Cities.AsNoTracking()
            .Where(c => c.CityId == cityId)
            .Select(c => new { c.PostalCode, c.CityName })
            .FirstOrDefaultAsync(cancellationToken);

        return city is null ? ("", "") : (city.PostalCode, city.CityName);
    }
}
