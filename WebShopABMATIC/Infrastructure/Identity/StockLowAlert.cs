namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class StockLowAlert
{
    public long Id { get; set; }
    public int ProductStockLocationId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int StockLocationId { get; set; }
    public string StockLocationName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal MinQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}
