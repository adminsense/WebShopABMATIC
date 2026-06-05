using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.StockLocations;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class StockLocationAdminService : IStockLocationAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public StockLocationAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<StockLocationDto>> GetStockLocationsAsync(StockLocationListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.StockLocations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Name.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new StockLocationDto
            {
                Id = e.Id,
                Name = e.Name,
                IsWarehouse = e.IsWarehouse
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<StockLocationDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<StockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.StockLocations.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new StockLocationEditDto
            {
                Id = e.Id,
                Name = e.Name,
                IsWarehouse = e.IsWarehouse ?? false
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(StockLocationEditDto dto, CancellationToken cancellationToken = default)
    {
        StockLocation entity;
        if (dto.Id == 0)
        {
            entity = (StockLocation)AdminCrudDefaults.Create("stock-locations");
            _db.StockLocations.Add(entity);
        }
        else
        {
            entity = await _db.StockLocations.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.IsWarehouse = dto.IsWarehouse;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.StockLocations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.StockLocations.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
