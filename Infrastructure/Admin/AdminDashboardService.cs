using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class AdminDashboardService : IAdminDashboardPort
{
    private readonly WebShopABMATICDbContext _db;

    public AdminDashboardService(WebShopABMATICDbContext db) => _db = db;

    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var productsOnWebshop = await SafeCountAsync(
            () => _db.Products.AsNoTracking().CountAsync(p => p.ShowOnWebshop == true, cancellationToken));

        var webshopNodes = await SafeCountAsync(
            () => _db.WebshopStructures.AsNoTracking().CountAsync(cancellationToken));

        var ordersThisMonth = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart, cancellationToken));

        var pendingOrders = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => !o.IsAccepted, cancellationToken));

        var lowStock = await SafeCountAsync(
            () => _db.ProductStockLocations.AsNoTracking()
                .CountAsync(x => x.Quantity <= x.MinQuantity, cancellationToken));

        var yearStart = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var revenueYtd = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= yearStart
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

        var costsYtd = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= yearStart
                   select (decimal?)(line.NettoAankoopPrijs * line.Quantity)).SumAsync(cancellationToken) ?? 0m);

        return new AdminDashboardDto
        {
            ProductsOnWebshop = productsOnWebshop,
            WebshopStructureNodes = webshopNodes,
            OrdersThisMonth = ordersThisMonth,
            PendingOrders = pendingOrders,
            LowStockAlerts = lowStock,
            RevenueYtd = revenueYtd,
            CostsYtd = costsYtd,
            NetYtd = revenueYtd - costsYtd
        };
    }

    private static async Task<int> SafeCountAsync(Func<Task<int>> query)
    {
        try
        {
            return await query();
        }
        catch
        {
            return 0;
        }
    }

    private static async Task<decimal> SafeSumAsync(Func<Task<decimal>> query)
    {
        try
        {
            return await query();
        }
        catch
        {
            return 0;
        }
    }
}
