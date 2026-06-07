using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Stock;

public sealed class LowStockAlertService : ILowStockAlertService
{
    private readonly WebShopABMATICDbContext _domainDb;
    private readonly ApplicationDbContext _appDb;
    private readonly ILogger<LowStockAlertService> _logger;

    public LowStockAlertService(
        WebShopABMATICDbContext domainDb,
        ApplicationDbContext appDb,
        ILogger<LowStockAlertService> logger)
    {
        _domainDb = domainDb;
        _appDb = appDb;
        _logger = logger;
    }

    public async Task EvaluateAsync(int productStockLocationId, decimal previousQuantity, CancellationToken cancellationToken = default)
    {
        var snapshot = await (
            from psl in _domainDb.ProductStockLocations.AsNoTracking()
            join product in _domainDb.Products.AsNoTracking() on psl.ProductId equals product.ProductId
            join location in _domainDb.StockLocations.AsNoTracking() on psl.StockLocationId equals location.Id
            where psl.Id == productStockLocationId && psl.IsInactive != true
            select new
            {
                psl.Id,
                psl.ProductId,
                ProductName = product.NameEn,
                psl.StockLocationId,
                StockLocationName = location.Name,
                psl.Quantity,
                psl.MinQuantity
            }).FirstOrDefaultAsync(cancellationToken);

        if (snapshot is null || snapshot.Quantity > snapshot.MinQuantity)
        {
            return;
        }

        var crossedThreshold = previousQuantity > snapshot.MinQuantity;
        var hasUnread = await _appDb.StockLowAlerts.AsNoTracking()
            .AnyAsync(a => a.ProductStockLocationId == productStockLocationId && !a.IsRead, cancellationToken);

        if (!crossedThreshold && hasUnread)
        {
            return;
        }

        _appDb.StockLowAlerts.Add(new StockLowAlert
        {
            ProductStockLocationId = snapshot.Id,
            ProductId = snapshot.ProductId,
            ProductName = snapshot.ProductName,
            StockLocationId = snapshot.StockLocationId,
            StockLocationName = snapshot.StockLocationName,
            Quantity = snapshot.Quantity,
            MinQuantity = snapshot.MinQuantity,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        });

        await _appDb.SaveChangesAsync(cancellationToken);

        _logger.LogWarning(
            "Low stock alert: product {ProductId} ({ProductName}) at {Location} — qty {Qty} / min {Min}",
            snapshot.ProductId,
            snapshot.ProductName,
            snapshot.StockLocationName,
            snapshot.Quantity,
            snapshot.MinQuantity);
    }

    public Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default) =>
        _appDb.StockLowAlerts.AsNoTracking().CountAsync(a => !a.IsRead, cancellationToken);

    public async Task<IReadOnlyList<StockLowAlertDto>> GetUnreadAlertsAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        var take = Math.Clamp(limit, 1, 50);

        return await _appDb.StockLowAlerts.AsNoTracking()
            .Where(a => !a.IsRead)
            .OrderByDescending(a => a.CreatedAt)
            .Take(take)
            .Select(a => new StockLowAlertDto
            {
                Id = a.Id,
                ProductStockLocationId = a.ProductStockLocationId,
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                StockLocationId = a.StockLocationId,
                StockLocationName = a.StockLocationName,
                Quantity = a.Quantity,
                MinQuantity = a.MinQuantity,
                CreatedAt = a.CreatedAt,
                Message = a.Quantity <= 0
                    ? $"{a.ProductName} is out of stock at {a.StockLocationName}."
                    : $"{a.ProductName} at {a.StockLocationName}: {a.Quantity} on hand (min {a.MinQuantity})."
            })
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAllReadAsync(CancellationToken cancellationToken = default)
    {
        var unread = await _appDb.StockLowAlerts
            .Where(a => !a.IsRead)
            .ToListAsync(cancellationToken);

        if (unread.Count == 0)
        {
            return;
        }

        var now = DateTime.UtcNow;
        foreach (var alert in unread)
        {
            alert.IsRead = true;
            alert.ReadAt = now;
        }

        await _appDb.SaveChangesAsync(cancellationToken);
    }
}
