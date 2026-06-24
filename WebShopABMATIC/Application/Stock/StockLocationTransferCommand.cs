namespace WebShopABMATIC.Application.Stock;

public sealed class StockLocationTransferCommand
{
    public required int ProductId { get; init; }
    public required int FromStockLocationId { get; init; }
    public required int ToStockLocationId { get; init; }
    public required decimal Quantity { get; init; }
    public required string Reason { get; init; }
}
