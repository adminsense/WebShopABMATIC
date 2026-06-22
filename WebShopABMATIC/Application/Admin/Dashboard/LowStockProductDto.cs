namespace WebShopABMATIC.Application.Admin.Dashboard;

public sealed class LowStockProductDto
{
    public int ProductStockLocationId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int StockLocationId { get; init; }
    public string StockLocationName { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal MinQuantity { get; init; }
    public bool IsOutOfStock => Quantity <= 0;
}
