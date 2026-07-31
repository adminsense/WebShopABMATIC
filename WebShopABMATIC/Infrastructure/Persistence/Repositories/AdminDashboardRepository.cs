using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class AdminDashboardRepository : IAdminDashboardRepository
{
    private readonly WebShopABMATICDbContext _db;

    public AdminDashboardRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var recentStart = now.Date.AddDays(-DashboardDefaults.RecentDays);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var totalProducts = await SafeCountAsync(
            () => _db.Products.AsNoTracking().CountAsync(p => !p.IsInactive, cancellationToken));
        var productsOnWebshop = await SafeCountAsync(
            () => _db.Products.AsNoTracking().CountAsync(p => !p.IsInactive && p.ShowOnWebshop == true, cancellationToken));
        var webshopNodes = await SafeCountAsync(
            () => _db.WebshopStructures.AsNoTracking().CountAsync(cancellationToken));
        var productImages = await SafeCountAsync(
            () => _db.AzureFiles.AsNoTracking()
                .CountAsync(f => f.ProductId != null && f.IsPrimaryImage == true, cancellationToken));
        var totalCustomers = await SafeCountAsync(
            () => _db.Customers.AsNoTracking().CountAsync(cancellationToken));
        var totalProjects = await SafeCountAsync(
            () => _db.Projects.AsNoTracking().CountAsync(cancellationToken));

        var ordersRecent = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= recentStart, cancellationToken));
        var pendingOrders = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => !o.IsAccepted && o.CreatedAt >= recentStart, cancellationToken));
        var ordersMonth = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart, cancellationToken));
        var acceptedOrdersMonth = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart && o.IsAccepted, cancellationToken));

        var itemsSoldRecent = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.CreatedAt >= recentStart
                   select (decimal?)line.Quantity).SumAsync(cancellationToken) ?? 0m);

        var revenueRecent = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= recentStart
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

        var pendingOrderValue = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where !order.IsAccepted && order.CreatedAt >= recentStart
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

        var costsRecent = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= recentStart
                   select (decimal?)(line.NettoAankoopPrijs * line.Quantity)).SumAsync(cancellationToken) ?? 0m);

        var lowStock = await SafeCountAsync(
            () => _db.ProductStockLocations.AsNoTracking()
                .CountAsync(x => x.Quantity <= x.MinQuantity, cancellationToken));
        var outOfStock = await SafeCountAsync(
            () => _db.ProductStockLocations.AsNoTracking()
                .CountAsync(x => x.Quantity <= 0, cancellationToken));
        var totalStockUnits = await SafeSumAsync(async () =>
            await _db.ProductStockLocations.AsNoTracking()
                .SumAsync(x => (decimal?)x.Quantity, cancellationToken) ?? 0m);
        var totalStockCapacity = await SafeSumAsync(async () =>
            await _db.ProductStockLocations.AsNoTracking()
                .SumAsync(x => (decimal?)x.MaxQuantity, cancellationToken) ?? 0m);
        var movementsRecent = await SafeCountAsync(
            () => _db.StockMovements.AsNoTracking().CountAsync(m => m.Timestamp >= recentStart, cancellationToken));
        var openPurchaseOrders = await SafeCountAsync(
            () => _db.StockOrders.AsNoTracking().CountAsync(o => !o.IsCompleted, cancellationToken));

        return new AdminDashboardDto
        {
            TotalProducts = totalProducts,
            ProductsOnWebshop = productsOnWebshop,
            WebshopStructureNodes = webshopNodes,
            ProductImages = productImages,
            TotalCustomers = totalCustomers,
            TotalProjects = totalProjects,
            OrdersThisMonth = ordersMonth,
            AcceptedOrdersThisMonth = acceptedOrdersMonth,
            PendingOrders = pendingOrders,
            OrdersYtd = ordersRecent,
            ItemsSoldThisMonth = itemsSoldRecent,
            ItemsSoldYtd = itemsSoldRecent,
            RevenueThisMonth = revenueRecent,
            PendingOrderValue = pendingOrderValue,
            LowStockAlerts = lowStock,
            OutOfStockProducts = outOfStock,
            TotalStockUnits = totalStockUnits,
            TotalStockCapacity = totalStockCapacity,
            StockMovementsLast7Days = movementsRecent,
            OpenPurchaseOrders = openPurchaseOrders,
            RevenueYtd = revenueRecent,
            CostsYtd = costsRecent,
            NetYtd = revenueRecent - costsRecent,
            PaidRevenueYtd = revenueRecent,
            OutstandingRevenueYtd = pendingOrderValue
        };
    }

    private static async Task<int> SafeCountAsync(Func<Task<int>> query)
    {
        try { return await query(); }
        catch { return 0; }
    }

    private static async Task<decimal> SafeSumAsync(Func<Task<decimal>> query)
    {
        try { return await query(); }
        catch { return 0; }
    }
}
