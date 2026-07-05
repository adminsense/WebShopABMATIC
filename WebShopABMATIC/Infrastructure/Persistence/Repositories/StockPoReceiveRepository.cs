using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StockPoReceiveRepository : IStockPoReceiveRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StockPoReceiveRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<StockPoReceiveContextDto?> GetReceiveContextAsync(
        int stockOrderId,
        CancellationToken cancellationToken = default)
    {
        var header = await (
            from order in _db.StockOrders.AsNoTracking()
            join supplier in _db.Suppliers.AsNoTracking() on order.SupplierId equals supplier.SupplierId into supplierJoin
            from supplier in supplierJoin.DefaultIfEmpty()
            where order.Id == stockOrderId
            select new
            {
                order.Id,
                order.IsCompleted,
                SupplierName = supplier != null ? supplier.Name : $"Supplier {order.SupplierId}"
            }).FirstOrDefaultAsync(cancellationToken);

        if (header is null)
        {
            return null;
        }

        var lines = await _db.StockOrderLines.AsNoTracking()
            .Where(l => l.StockOrderId == stockOrderId)
            .OrderBy(l => l.Id)
            .Select(l => new StockPoReceiveLineDto
            {
                LineId = l.Id,
                ProductId = l.ProductId,
                ProductName = l.ProductName,
                QuantityOrdered = l.QuantityOrdered,
                QuantityDelivered = l.QuantityDelivered,
                QuantityRemaining = l.QuantityOrdered - l.QuantityDelivered,
                IsFullyDelivered = l.Geleverd == true || l.QuantityDelivered >= l.QuantityOrdered
            })
            .ToListAsync(cancellationToken);

        var locations = await _db.StockLocations.AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => new StockLookupItemDto { Id = l.Id, Name = l.Name })
            .ToListAsync(cancellationToken);

        return new StockPoReceiveContextDto
        {
            StockOrderId = header.Id,
            SupplierName = header.SupplierName,
            IsCompleted = header.IsCompleted,
            Lines = lines,
            StockLocations = locations
        };
    }

    public async Task<StockPoReceivePreviewDto?> GetPreviewAsync(
        int stockOrderLineId,
        int stockLocationId,
        CancellationToken cancellationToken = default)
    {
        var line = await _db.StockOrderLines.AsNoTracking()
            .Where(l => l.Id == stockOrderLineId)
            .Select(l => new
            {
                l.Id,
                l.ProductId,
                l.ProductName,
                l.QuantityOrdered,
                l.QuantityDelivered
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (line is null || line.ProductId is not int productId)
        {
            return null;
        }

        var stockRow = await (
            from row in _db.ProductStockLocations.AsNoTracking()
            join location in _db.StockLocations.AsNoTracking() on row.StockLocationId equals location.Id
            where row.ProductId == productId
                  && row.StockLocationId == stockLocationId
                  && row.IsInactive != true
            select new
            {
                row.StockLocationId,
                location.Name,
                row.Quantity
            }).FirstOrDefaultAsync(cancellationToken);

        if (stockRow is null)
        {
            return null;
        }

        return new StockPoReceivePreviewDto
        {
            LineId = line.Id,
            ProductId = productId,
            ProductName = line.ProductName,
            StockLocationId = stockRow.StockLocationId,
            StockLocationName = stockRow.Name,
            CurrentQuantity = stockRow.Quantity,
            QuantityRemaining = line.QuantityOrdered - line.QuantityDelivered
        };
    }
}
