using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductStockLocations;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class ProductStockLocationAdminService : IProductStockLocationAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public ProductStockLocationAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductStockLocationDto>> GetProductStockLocationsAsync(ProductStockLocationListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.ProductStockLocations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search) && int.TryParse(filter.Search.Trim(), out var productId))
        {
            query = query.Where(e => e.ProductId == productId);
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProductStockLocationDto
            {
                Id = e.Id,
                StockLocationId = e.StockLocationId,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                MinQuantity = e.MinQuantity,
                MaxQuantity = e.MaxQuantity,
                IsDefault = e.IsDefault
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductStockLocationDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ProductStockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.ProductStockLocations.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ProductStockLocationEditDto
            {
                Id = e.Id,
                StockLocationId = e.StockLocationId,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                MinQuantity = e.MinQuantity,
                MaxQuantity = e.MaxQuantity,
                IsDefault = e.IsDefault
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(ProductStockLocationEditDto dto, CancellationToken cancellationToken = default)
    {
        ProductStockLocation entity;
        if (dto.Id == 0)
        {
            entity = (ProductStockLocation)AdminCrudDefaults.Create("product-stock");
            _db.ProductStockLocations.Add(entity);
        }
        else
        {
            entity = await _db.ProductStockLocations.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.StockLocationId = dto.StockLocationId;
        entity.ProductId = dto.ProductId;
        entity.Quantity = dto.Quantity;
        entity.MinQuantity = dto.MinQuantity;
        entity.MaxQuantity = dto.MaxQuantity;
        entity.IsDefault = dto.IsDefault;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductStockLocations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.ProductStockLocations.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
