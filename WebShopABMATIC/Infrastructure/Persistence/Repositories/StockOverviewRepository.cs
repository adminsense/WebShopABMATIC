using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StockOverviewRepository : IStockOverviewRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StockOverviewRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<StockOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var sevenDaysAgo = today.AddDays(-7);

        var stockRows = await _db.ProductStockLocations.AsNoTracking().ToListAsync(cancellationToken);

        var skusInStock = stockRows.Count(x => x.Quantity > 0);
        var lowStock = stockRows.Count(x => x.Quantity <= x.MinQuantity);
        var outOfStock = stockRows.Count(x => x.Quantity <= 0);
        var overstock = stockRows.Count(x => x.MaxQuantity > 0 && x.Quantity > x.MaxQuantity);
        var totalOnHand = stockRows.Sum(x => x.Quantity);
        var totalReserved = stockRows.Sum(x => x.ReservedQuantity);

        var movementsToday = await _db.StockMovements.AsNoTracking()
            .CountAsync(m => m.Timestamp >= today, cancellationToken);

        var movementsLast7Days = await _db.StockMovements.AsNoTracking()
            .CountAsync(m => m.Timestamp >= sevenDaysAgo, cancellationToken);

        var openPos = await _db.StockOrders.AsNoTracking()
            .CountAsync(o => !o.IsCompleted, cancellationToken);

        var poLinesAwaiting = await _db.StockOrderLines.AsNoTracking()
            .CountAsync(l => l.Geleverd != true && l.QuantityDelivered < l.QuantityOrdered, cancellationToken);

        var locationNames = await _db.StockLocations.AsNoTracking()
            .ToDictionaryAsync(l => l.Id, l => l.Name, cancellationToken);

        var locationBalances = stockRows
            .GroupBy(x => x.StockLocationId)
            .Select(g =>
            {
                var onHand = g.Sum(x => x.Quantity);
                var reserved = g.Sum(x => x.ReservedQuantity);
                return new StockLocationBalanceDto
                {
                    StockLocationId = g.Key,
                    LocationName = locationNames.GetValueOrDefault(g.Key, $"Location {g.Key}"),
                    OnHand = onHand,
                    Reserved = reserved,
                    Available = onHand - reserved
                };
            })
            .OrderBy(x => x.LocationName)
            .ToList();

        return new StockOverviewDto
        {
            SkusInStock = skusInStock,
            LowStockCount = lowStock,
            OutOfStockCount = outOfStock,
            OverstockCount = overstock,
            TotalOnHand = totalOnHand,
            TotalReserved = totalReserved,
            TotalAvailable = totalOnHand - totalReserved,
            MovementsToday = movementsToday,
            MovementsLast7Days = movementsLast7Days,
            OpenPurchaseOrders = openPos,
            PoLinesAwaitingDelivery = poLinesAwaiting,
            LocationBalances = locationBalances
        };
    }
}
