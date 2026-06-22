namespace WebShopABMATIC.Application.Store;

public sealed class StoreProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = "/images/product1.png";
    public decimal? Price { get; init; }
    public bool HasPrice => Price.HasValue;
    public int Stock { get; init; }
    public decimal MinQuantity { get; init; }
    public bool IsLowStock => MinQuantity > 0 && Stock <= MinQuantity;
    public bool IsOutOfStock => Stock <= 0;
    public int? CategoryId { get; init; }
    public int? CategoryRootId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string Tag => string.IsNullOrWhiteSpace(CategoryName) ? "Product" : CategoryName;
}
