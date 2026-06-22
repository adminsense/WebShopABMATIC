using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class AdminDashboardUseCase : IAdminDashboardPort
{
    private readonly IAdminDashboardRepository _repository;
    private readonly ILowStockReadRepository _lowStock;
    private readonly ILowStockAlertService _alerts;

    public AdminDashboardUseCase(
        IAdminDashboardRepository repository,
        ILowStockReadRepository lowStock,
        ILowStockAlertService alerts)
    {
        _repository = repository;
        _lowStock = lowStock;
        _alerts = alerts;
    }

    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var core = await _repository.GetDashboardAsync(cancellationToken);

        IReadOnlyList<LowStockProductDto> lowStockProducts = [];
        IReadOnlyList<StockLowAlertDto> unreadAlerts = [];
        var unreadCount = 0;

        try
        {
            lowStockProducts = await _lowStock.GetLowStockProductsAsync(25, cancellationToken);
            unreadAlerts = await _alerts.GetUnreadAlertsAsync(10, cancellationToken);
            unreadCount = await _alerts.GetUnreadCountAsync(cancellationToken);
        }
        catch
        {
            // Dashboard core metrics still load when optional widgets fail.
        }

        return new AdminDashboardDto
        {
            TotalProducts = core.TotalProducts,
            ProductsOnWebshop = core.ProductsOnWebshop,
            WebshopStructureNodes = core.WebshopStructureNodes,
            ProductImages = core.ProductImages,
            TotalCustomers = core.TotalCustomers,
            TotalProjects = core.TotalProjects,
            OrdersThisMonth = core.OrdersThisMonth,
            AcceptedOrdersThisMonth = core.AcceptedOrdersThisMonth,
            PendingOrders = core.PendingOrders,
            OrdersYtd = core.OrdersYtd,
            ItemsSoldThisMonth = core.ItemsSoldThisMonth,
            ItemsSoldYtd = core.ItemsSoldYtd,
            RevenueThisMonth = core.RevenueThisMonth,
            PendingOrderValue = core.PendingOrderValue,
            LowStockAlerts = core.LowStockAlerts,
            OutOfStockProducts = core.OutOfStockProducts,
            TotalStockUnits = core.TotalStockUnits,
            TotalStockCapacity = core.TotalStockCapacity,
            StockMovementsLast7Days = core.StockMovementsLast7Days,
            OpenPurchaseOrders = core.OpenPurchaseOrders,
            RevenueYtd = core.RevenueYtd,
            CostsYtd = core.CostsYtd,
            NetYtd = core.NetYtd,
            PaidRevenueYtd = core.PaidRevenueYtd,
            OutstandingRevenueYtd = core.OutstandingRevenueYtd,
            LowStockProducts = lowStockProducts,
            UnreadStockAlerts = unreadAlerts,
            UnreadStockAlertCount = unreadCount
        };
    }

    public Task MarkStockAlertsReadAsync(CancellationToken cancellationToken = default) =>
        _alerts.MarkAllReadAsync(cancellationToken);
}
