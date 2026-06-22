namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockLowAlertDto
{
    public long Id { get; init; }
    public int ProductStockLocationId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int StockLocationId { get; init; }
    public string StockLocationName { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal MinQuantity { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Message { get; init; } = string.Empty;
}
