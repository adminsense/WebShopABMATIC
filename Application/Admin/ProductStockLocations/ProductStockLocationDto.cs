namespace WebShopABMATIC.Application.Admin.ProductStockLocations;

public sealed class ProductStockLocationDto
{
    public int Id { get; init; }
    public int StockLocationId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal ReservedQuantity { get; init; }
    public decimal AvailableQuantity => Math.Max(0, Quantity - ReservedQuantity);
    public decimal MinQuantity { get; init; }
    public decimal MaxQuantity { get; init; }
    public bool IsDefault { get; init; }
    public bool IsLowStock => Quantity <= MinQuantity;
}

public sealed class ProductStockLocationEditDto
{
    public int Id { get; set; }
    public int StockLocationId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal MinQuantity { get; set; }
    public decimal MaxQuantity { get; set; }
    public bool IsDefault { get; set; }
}

public sealed class ProductStockLocationListFilter
{
    public string? Search { get; set; }
    public bool LowStockOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
