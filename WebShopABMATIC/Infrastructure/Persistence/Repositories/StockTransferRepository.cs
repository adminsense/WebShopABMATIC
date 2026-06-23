using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StockTransferRepository : IStockTransferRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StockTransferRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<StockTransferLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _db.StockLocations.AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => new StockLookupItemDto { Id = l.Id, Name = l.Name })
            .ToListAsync(cancellationToken);

        return new StockTransferLookupsDto { StockLocations = locations };
    }

    public async Task<StockTransferPreviewDto?> GetPreviewAsync(
        int productId,
        int fromStockLocationId,
        int toStockLocationId,
        CancellationToken cancellationToken = default)
    {
        if (fromStockLocationId == toStockLocationId)
        {
            return null;
        }

        var productName = await _db.Products.AsNoTracking()
            .Where(p => p.ProductId == productId)
            .Select(p => p.NameEn)
            .FirstOrDefaultAsync(cancellationToken);

        if (productName is null)
        {
            return null;
        }

        var rows = await (
            from row in _db.ProductStockLocations.AsNoTracking()
            join location in _db.StockLocations.AsNoTracking() on row.StockLocationId equals location.Id
            where row.ProductId == productId
                  && (row.StockLocationId == fromStockLocationId || row.StockLocationId == toStockLocationId)
                  && row.IsInactive != true
            select new
            {
                row.StockLocationId,
                location.Name,
                row.Quantity
            }).ToListAsync(cancellationToken);

        var from = rows.FirstOrDefault(r => r.StockLocationId == fromStockLocationId);
        var to = rows.FirstOrDefault(r => r.StockLocationId == toStockLocationId);

        if (from is null || to is null)
        {
            return null;
        }

        return new StockTransferPreviewDto
        {
            ProductId = productId,
            ProductName = productName,
            FromStockLocationId = from.StockLocationId,
            FromStockLocationName = from.Name,
            FromCurrentQuantity = from.Quantity,
            ToStockLocationId = to.StockLocationId,
            ToStockLocationName = to.Name,
            ToCurrentQuantity = to.Quantity
        };
    }
}
