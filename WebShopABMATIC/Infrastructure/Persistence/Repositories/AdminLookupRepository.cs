using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Lookups;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class AdminLookupRepository : IAdminLookupRepository
{
    private readonly WebShopABMATICDbContext _db;

    public AdminLookupRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<CityLookupDto>> SearchCitiesAsync(CityListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Cities.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            if (int.TryParse(term, out var cityId))
            {
                query = query.Where(c =>
                    c.CityId == cityId ||
                    (c.CityName != null && c.CityName.Contains(term)) ||
                    (c.PostalCode != null && c.PostalCode.Contains(term)));
            }
            else
            {
                query = query.Where(c =>
                    (c.CityName != null && c.CityName.Contains(term)) ||
                    (c.PostalCode != null && c.PostalCode.Contains(term)) ||
                    (c.CountryName != null && c.CountryName.Contains(term)));
            }
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(c => c.CityName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CityLookupDto
            {
                CityId = c.CityId,
                CityName = c.CityName ?? string.Empty,
                PostalCode = c.PostalCode ?? string.Empty,
                CountryName = c.CountryName ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CityLookupDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<CityLookupDto?> GetCityAsync(int cityId, CancellationToken cancellationToken = default) =>
        await _db.Cities.AsNoTracking()
            .Where(c => c.CityId == cityId)
            .Select(c => new CityLookupDto
            {
                CityId = c.CityId,
                CityName = c.CityName ?? string.Empty,
                PostalCode = c.PostalCode ?? string.Empty,
                CountryName = c.CountryName ?? string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<LanguageLookupDto>> GetLanguagesAsync(CancellationToken cancellationToken = default) =>
        await _db.Languages.AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => new LanguageLookupDto { Id = l.Id, Name = l.Name ?? string.Empty })
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NamedLookupDto>> GetDeliveryTypesAsync(CancellationToken cancellationToken = default) =>
        await _db.DeliveryTypes.AsNoTracking()
            .OrderBy(d => d.Name)
            .Select(d => new NamedLookupDto { Id = d.Id, Name = d.Name ?? string.Empty })
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NamedLookupDto>> GetCustomerTypesAsync(CancellationToken cancellationToken = default) =>
        await _db.CustomerTypes.AsNoTracking()
            .OrderBy(t => t.CustomerTypeName)
            .Select(t => new NamedLookupDto { Id = t.KlantTypeId, Name = t.CustomerTypeName ?? string.Empty })
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NamedLookupDto>> GetSuppliersAsync(CancellationToken cancellationToken = default) =>
        await _db.Suppliers.AsNoTracking()
            .Where(s => s.IsInactive != true)
            .OrderBy(s => s.Name)
            .Select(s => new NamedLookupDto { Id = s.SupplierId, Name = s.Name ?? string.Empty })
            .Take(500)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NamedLookupDto>> GetManufacturersAsync(CancellationToken cancellationToken = default) =>
        await _db.Manufacturers.AsNoTracking()
            .OrderBy(m => m.Name)
            .Select(m => new NamedLookupDto { Id = m.ManufacturerId, Name = m.Name ?? string.Empty })
            .Take(500)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<NamedLookupDto>> GetStockLocationsAsync(CancellationToken cancellationToken = default) =>
        await _db.StockLocations.AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => new NamedLookupDto { Id = l.Id, Name = l.Name ?? string.Empty })
            .ToListAsync(cancellationToken);

    public async Task EnsureCityExistsAsync(int cityId, CancellationToken cancellationToken = default)
    {
        if (cityId <= 0)
        {
            throw new InvalidOperationException("Select a city before saving.");
        }

        var exists = await _db.Cities.AsNoTracking().AnyAsync(c => c.CityId == cityId, cancellationToken);
        if (!exists)
        {
            throw new InvalidOperationException($"City #{cityId} was not found.");
        }
    }
}
