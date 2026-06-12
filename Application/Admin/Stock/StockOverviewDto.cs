namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockOverviewDto
{
    public int SkusInStock { get; init; }
    public int LowStockCount { get; init; }
    public int OutOfStockCount { get; init; }
    public int OverstockCount { get; init; }
    public decimal TotalOnHand { get; init; }
    public decimal TotalReserved { get; init; }
    public decimal TotalAvailable { get; init; }
    public int MovementsToday { get; init; }
    public int MovementsLast7Days { get; init; }
    public int OpenPurchaseOrders { get; init; }
    public int PoLinesAwaitingDelivery { get; init; }
    public IReadOnlyList<StockLocationBalanceDto> LocationBalances { get; init; } = [];
}

public sealed class StockLocationBalanceDto
{
    public int StockLocationId { get; init; }
    public string LocationName { get; init; } = "";
    public decimal OnHand { get; init; }
    public decimal Reserved { get; init; }
    public decimal Available { get; init; }
}
