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

        var totalProductsTask = SafeCountAsync(
            () => _db.Products.AsNoTracking().CountAsync(p => !p.IsInactive, cancellationToken));
        var productsOnWebshopTask = SafeCountAsync(
            () => _db.Products.AsNoTracking().CountAsync(p => !p.IsInactive && p.ShowOnWebshop == true, cancellationToken));
        var webshopNodesTask = SafeCountAsync(
            () => _db.WebshopStructures.AsNoTracking().CountAsync(cancellationToken));
        var productImagesTask = SafeCountAsync(
            () => _db.AzureFiles.AsNoTracking()
                .CountAsync(f => f.ProductId != null && f.IsPrimaryImage == true, cancellationToken));
        var totalCustomersTask = SafeCountAsync(
            () => _db.Customers.AsNoTracking().CountAsync(cancellationToken));
        var totalProjectsTask = SafeCountAsync(
            () => _db.Projects.AsNoTracking().CountAsync(cancellationToken));

        var ordersRecentTask = SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= recentStart, cancellationToken));
        var acceptedOrdersRecentTask = SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= recentStart && o.IsAccepted, cancellationToken));
        var pendingOrdersTask = SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => !o.IsAccepted && o.CreatedAt >= recentStart, cancellationToken));
        var ordersMonthTask = SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart, cancellationToken));
        var acceptedOrdersMonthTask = SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart && o.IsAccepted, cancellationToken));

        var itemsSoldRecentTask = SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.CreatedAt >= recentStart
                   select (decimal?)line.Quantity).SumAsync(cancellationToken) ?? 0m);

        var revenueRecentTask = SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= recentStart
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

        var pendingOrderValueTask = SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where !order.IsAccepted && order.CreatedAt >= recentStart
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

        var costsRecentTask = SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= recentStart
                   select (decimal?)(line.NettoAankoopPrijs * line.Quantity)).SumAsync(cancellationToken) ?? 0m);

        var lowStockTask = SafeCountAsync(
            () => _db.ProductStockLocations.AsNoTracking()
                .CountAsync(x => x.Quantity <= x.MinQuantity, cancellationToken));
        var outOfStockTask = SafeCountAsync(
            () => _db.ProductStockLocations.AsNoTracking()
                .CountAsync(x => x.Quantity <= 0, cancellationToken));
        var totalStockUnitsTask = SafeSumAsync(async () =>
            await _db.ProductStockLocations.AsNoTracking()
                .SumAsync(x => (decimal?)x.Quantity, cancellationToken) ?? 0m);
        var totalStockCapacityTask = SafeSumAsync(async () =>
            await _db.ProductStockLocations.AsNoTracking()
                .SumAsync(x => (decimal?)x.MaxQuantity, cancellationToken) ?? 0m);
        var movementsRecentTask = SafeCountAsync(
            () => _db.StockMovements.AsNoTracking().CountAsync(m => m.Timestamp >= recentStart, cancellationToken));
        var openPurchaseOrdersTask = SafeCountAsync(
            () => _db.StockOrders.AsNoTracking().CountAsync(o => !o.IsCompleted, cancellationToken));

        await Task.WhenAll(
            totalProductsTask, productsOnWebshopTask, webshopNodesTask, productImagesTask,
            totalCustomersTask, totalProjectsTask, ordersRecentTask, acceptedOrdersRecentTask,
            pendingOrdersTask, ordersMonthTask, acceptedOrdersMonthTask, itemsSoldRecentTask,
            revenueRecentTask, pendingOrderValueTask, costsRecentTask, lowStockTask, outOfStockTask,
            totalStockUnitsTask, totalStockCapacityTask, movementsRecentTask, openPurchaseOrdersTask);

        var revenueRecent = await revenueRecentTask;
        var costsRecent = await costsRecentTask;

        return new AdminDashboardDto
        {
            TotalProducts = await totalProductsTask,
            ProductsOnWebshop = await productsOnWebshopTask,
            WebshopStructureNodes = await webshopNodesTask,
            ProductImages = await productImagesTask,
            TotalCustomers = await totalCustomersTask,
            TotalProjects = await totalProjectsTask,
            OrdersThisMonth = await ordersMonthTask,
            AcceptedOrdersThisMonth = await acceptedOrdersMonthTask,
            PendingOrders = await pendingOrdersTask,
            OrdersYtd = await ordersRecentTask,
            ItemsSoldThisMonth = await itemsSoldRecentTask,
            ItemsSoldYtd = await itemsSoldRecentTask,
            RevenueThisMonth = revenueRecent,
            PendingOrderValue = await pendingOrderValueTask,
            LowStockAlerts = await lowStockTask,
            OutOfStockProducts = await outOfStockTask,
            TotalStockUnits = await totalStockUnitsTask,
            TotalStockCapacity = await totalStockCapacityTask,
            StockMovementsLast7Days = await movementsRecentTask,
            OpenPurchaseOrders = await openPurchaseOrdersTask,
            RevenueYtd = revenueRecent,
            CostsYtd = costsRecent,
            NetYtd = revenueRecent - costsRecent,
            PaidRevenueYtd = revenueRecent,
            OutstandingRevenueYtd = await pendingOrderValueTask
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
