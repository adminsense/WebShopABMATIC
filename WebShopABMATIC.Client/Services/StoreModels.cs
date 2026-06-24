namespace WebShopABMATIC.Web.Services;

public sealed record StoreProduct(
    int Id,
    string Name,
    string Description,
    string ImageUrl,
    decimal? Price,
    bool HasPrice,
    int Stock,
    decimal MinQuantity,
    bool IsLowStock,
    bool IsOutOfStock,
    int? CategoryId,
    int? CategoryRootId,
    string CategoryName,
    string Tag);

public sealed class CartLine
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}

public sealed record StoreOrderSummary(
    string OrderNumber,
    DateOnly Date,
    string Status,
    string StatusClass,
    string ItemsSummary,
    decimal Total);

public static class StorePriceFormatter
{
    public const string OnRequestLabel = "Price on request";

    public static string Format(decimal? price) =>
        price.HasValue ? $"€{price.Value:F2}" : OnRequestLabel;
}
