using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductStockLocations;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductStockLocationRepository : IProductStockLocationRepository
{
    private readonly WebShopABMATICDbContext _db;
    private readonly ILowStockAlertService _lowStockAlerts;

    public ProductStockLocationRepository(WebShopABMATICDbContext db, ILowStockAlertService lowStockAlerts)
    {
        _db = db;
        _lowStockAlerts = lowStockAlerts;
    }

    public async Task<PagedResult<ProductStockLocationDto>> GetProductStockLocationsAsync(
        ProductStockLocationListFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query =
            from psl in _db.ProductStockLocations.AsNoTracking()
            join product in _db.Products.AsNoTracking() on psl.ProductId equals product.ProductId into productJoin
            from product in productJoin.DefaultIfEmpty()
            select new { psl, ProductName = product != null ? product.NameEn : $"Product {psl.ProductId}" };

        if (!string.IsNullOrWhiteSpace(filter.Search) && int.TryParse(filter.Search.Trim(), out var productId))
        {
            query = query.Where(x => x.psl.ProductId == productId);
        }

        if (filter.LowStockOnly)
        {
            query = query.Where(x => x.psl.Quantity <= x.psl.MinQuantity);
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(x => x.psl.Quantity)
            .ThenBy(x => x.psl.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductStockLocationDto
            {
                Id = x.psl.Id,
                StockLocationId = x.psl.StockLocationId,
                ProductId = x.psl.ProductId,
                ProductName = x.ProductName,
                Quantity = x.psl.Quantity,
                ReservedQuantity = x.psl.ReservedQuantity,
                MinQuantity = x.psl.MinQuantity,
                MaxQuantity = x.psl.MaxQuantity,
                IsDefault = x.psl.IsDefault
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
        decimal previousQuantity = 0;

        if (dto.Id == 0)
        {
            entity = (ProductStockLocation)AdminCrudDefaults.Create("product-stock");
            _db.ProductStockLocations.Add(entity);
        }
        else
        {
            entity = await _db.ProductStockLocations.FirstAsync(e => e.Id == dto.Id, cancellationToken);
            previousQuantity = entity.Quantity;
        }

        entity.StockLocationId = dto.StockLocationId;
        entity.ProductId = dto.ProductId;
        entity.Quantity = dto.Quantity;
        entity.MinQuantity = dto.MinQuantity;
        entity.MaxQuantity = dto.MaxQuantity;
        entity.IsDefault = dto.IsDefault;

        await _db.SaveChangesAsync(cancellationToken);

        await _lowStockAlerts.EvaluateAsync(entity.Id, previousQuantity, cancellationToken);

        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductStockLocations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        _db.ProductStockLocations.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
