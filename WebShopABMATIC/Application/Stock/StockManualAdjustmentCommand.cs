namespace WebShopABMATIC.Application.Stock;

public sealed class StockManualAdjustmentCommand
{
    public required int ProductId { get; init; }
    public required int StockLocationId { get; init; }
    public required decimal QuantityChange { get; init; }
    public required string Reason { get; init; }
}
