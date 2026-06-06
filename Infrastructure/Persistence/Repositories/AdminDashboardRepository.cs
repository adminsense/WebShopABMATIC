using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Dashboard;
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
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var yearStart = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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

        var ordersThisMonth = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart, cancellationToken));

        var acceptedOrdersThisMonth = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= monthStart && o.IsAccepted, cancellationToken));

        var pendingOrders = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => !o.IsAccepted, cancellationToken));

        var ordersYtd = await SafeCountAsync(
            () => _db.Orders.AsNoTracking().CountAsync(o => o.CreatedAt >= yearStart, cancellationToken));

        var itemsSoldThisMonth = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.CreatedAt >= monthStart
                   select (decimal?)line.Quantity).SumAsync(cancellationToken) ?? 0m);

        var itemsSoldYtd = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.CreatedAt >= yearStart
                   select (decimal?)line.Quantity).SumAsync(cancellationToken) ?? 0m);

        var revenueThisMonth = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where order.IsAccepted && order.CreatedAt >= monthStart
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

        var pendingOrderValue = await SafeSumAsync(async () =>
            await (from line in _db.OrderLines.AsNoTracking()
                   join order in _db.Orders.AsNoTracking() on line.OrderId equals order.Id
                   where !order.IsAccepted
                   select (decimal?)line.TotalExclVat).SumAsync(cancellationToken) ?? 0m);

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

        var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
        var movementsLast7Days = await SafeCountAsync(
            () => _db.StockMovements.AsNoTracking().CountAsync(m => m.Timestamp >= sevenDaysAgo, cancellationToken));

        var openPurchaseOrders = await SafeCountAsync(
            () => _db.StockOrders.AsNoTracking().CountAsync(o => !o.IsCompleted, cancellationToken));

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

        var paidRevenueYtd = revenueYtd;
        var outstandingRevenueYtd = pendingOrderValue;

        return new AdminDashboardDto
        {
            TotalProducts = totalProducts,
            ProductsOnWebshop = productsOnWebshop,
            WebshopStructureNodes = webshopNodes,
            ProductImages = productImages,
            TotalCustomers = totalCustomers,
            TotalProjects = totalProjects,
            OrdersThisMonth = ordersThisMonth,
            AcceptedOrdersThisMonth = acceptedOrdersThisMonth,
            PendingOrders = pendingOrders,
            OrdersYtd = ordersYtd,
            ItemsSoldThisMonth = itemsSoldThisMonth,
            ItemsSoldYtd = itemsSoldYtd,
            RevenueThisMonth = revenueThisMonth,
            PendingOrderValue = pendingOrderValue,
            LowStockAlerts = lowStock,
            OutOfStockProducts = outOfStock,
            TotalStockUnits = totalStockUnits,
            TotalStockCapacity = totalStockCapacity,
            StockMovementsLast7Days = movementsLast7Days,
            OpenPurchaseOrders = openPurchaseOrders,
            RevenueYtd = revenueYtd,
            CostsYtd = costsYtd,
            NetYtd = revenueYtd - costsYtd,
            PaidRevenueYtd = paidRevenueYtd,
            OutstandingRevenueYtd = outstandingRevenueYtd
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
