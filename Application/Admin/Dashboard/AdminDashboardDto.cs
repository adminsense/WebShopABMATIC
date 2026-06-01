namespace WebShopABMATIC.Application.Admin.Dashboard;

public sealed class AdminDashboardDto
{
    public int ProductsOnWebshop { get; init; }
    public int WebshopStructureNodes { get; init; }
    public int OrdersThisMonth { get; init; }
    public int PendingOrders { get; init; }
    public int LowStockAlerts { get; init; }
    public decimal RevenueYtd { get; init; }
    public decimal CostsYtd { get; init; }
    public decimal NetYtd { get; init; }
}
