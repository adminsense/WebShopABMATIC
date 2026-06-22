using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Application.Admin.Dashboard;

public sealed class AdminDashboardDto
{    public int TotalProducts { get; init; }
    public int ProductsOnWebshop { get; init; }
    public int WebshopStructureNodes { get; init; }
    public int ProductImages { get; init; }
    public int TotalCustomers { get; init; }
    public int TotalProjects { get; init; }

    public int OrdersThisMonth { get; init; }
    public int AcceptedOrdersThisMonth { get; init; }
    public int PendingOrders { get; init; }
    public int OrdersYtd { get; init; }
    public decimal ItemsSoldThisMonth { get; init; }
    public decimal ItemsSoldYtd { get; init; }
    public decimal RevenueThisMonth { get; init; }
    public decimal PendingOrderValue { get; init; }

    public int LowStockAlerts { get; init; }
    public int OutOfStockProducts { get; init; }
    public decimal TotalStockUnits { get; init; }
    public decimal TotalStockCapacity { get; init; }
    public int StockMovementsLast7Days { get; init; }
    public int OpenPurchaseOrders { get; init; }

    public decimal RevenueYtd { get; init; }
    public decimal CostsYtd { get; init; }
    public decimal NetYtd { get; init; }
    public decimal PaidRevenueYtd { get; init; }
    public decimal OutstandingRevenueYtd { get; init; }

    public IReadOnlyList<LowStockProductDto> LowStockProducts { get; init; } = [];
    public IReadOnlyList<StockLowAlertDto> UnreadStockAlerts { get; init; } = [];
    public int UnreadStockAlertCount { get; init; }
}