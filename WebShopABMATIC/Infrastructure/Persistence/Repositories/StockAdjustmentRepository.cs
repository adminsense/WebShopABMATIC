using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StockAdjustmentRepository : IStockAdjustmentRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StockAdjustmentRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<StockAdjustmentLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _db.StockLocations.AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => new StockLookupItemDto { Id = l.Id, Name = l.Name })
            .ToListAsync(cancellationToken);

        return new StockAdjustmentLookupsDto { StockLocations = locations };
    }

    public async Task<StockAdjustmentPreviewDto?> GetPreviewAsync(
        int productId,
        int stockLocationId,
        CancellationToken cancellationToken = default)
    {
        return await (
            from row in _db.ProductStockLocations.AsNoTracking()
            join product in _db.Products.AsNoTracking() on row.ProductId equals product.ProductId
            join location in _db.StockLocations.AsNoTracking() on row.StockLocationId equals location.Id
            where row.ProductId == productId
                  && row.StockLocationId == stockLocationId
                  && row.IsInactive != true
            select new StockAdjustmentPreviewDto
            {
                ProductId = row.ProductId,
                ProductName = product.NameEn,
                StockLocationId = row.StockLocationId,
                StockLocationName = location.Name,
                CurrentQuantity = row.Quantity,
                ProductStockLocationId = row.Id
            }).FirstOrDefaultAsync(cancellationToken);
    }
}
