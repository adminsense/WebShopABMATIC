namespace WebShopABMATIC.Application.Stock;

public sealed class StockPoReceiveCommand
{
    public required int StockOrderLineId { get; init; }
    public required int StockLocationId { get; init; }
    public required string DeliveryDocumentNumber { get; init; }
    public required DateTime DeliveryDate { get; init; }
    public required decimal Quantity { get; init; }
}
