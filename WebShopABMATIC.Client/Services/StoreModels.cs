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
    bool IsNew,
    int? CategoryId,
    int? CategoryRootId,
    string CategoryName,
    string ReferenceCode,
    string Tag,
    bool HasOptions = false);

public sealed class CartLine
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public List<CartLineOption> Options { get; set; } = [];
    public decimal LineTotal => UnitPrice * Quantity;
    public bool HasOptions => Options.Count > 0;
    public string OptionsSummary => string.Join(", ", Options.Select(o => $"{o.OptionName}: {o.ValueText}"));

    /// <summary>Stable signature used to merge identical lines (same product + same option selection).</summary>
    public string Signature =>
        $"{ProductId}|{string.Join(";", Options.OrderBy(o => o.OptionId).Select(o => $"{o.OptionId}={o.ValueId?.ToString() ?? o.ValueText}"))}";
}

public sealed class CartLineOption
{
    public int OptionId { get; set; }
    public string OptionName { get; set; } = "";
    public int? ValueId { get; set; }
    public string ValueText { get; set; } = "";
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
    public const string LoginForPriceLabel = "Meld u aan om uw prijs te zien";

    public static string Format(decimal? price) =>
        price.HasValue ? $"€{price.Value:F2}" : OnRequestLabel;
}
