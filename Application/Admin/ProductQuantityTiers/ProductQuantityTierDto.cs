namespace WebShopABMATIC.Application.Admin.ProductQuantityTiers;

public sealed class ProductQuantityTierDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public decimal MinimumQuantity { get; init; }
    public decimal Discount { get; init; }
}

public sealed class ProductQuantityTierEditDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal MinimumQuantity { get; set; }
    public decimal Discount { get; set; }
}

public sealed class ProductQuantityTierListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
