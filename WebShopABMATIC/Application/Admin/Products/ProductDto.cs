using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.Products;

public sealed class ProductDto
{
    public int ProductId { get; init; }
    public string NameEn { get; init; } = string.Empty;
    public string? OrderPartNumber { get; init; }
    public int SupplierId { get; init; }
    public int ManufacturerId { get; init; }
    public bool ShowOnWebshop { get; init; }
    public string? WebshopDescriptionNl { get; init; }
    public string? EanCode { get; init; }
    public bool IsInactive { get; init; }
}

public sealed class ProductEditDto
{
    public int ProductId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string OrderPartNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public int ManufacturerId { get; set; }
    public bool ShowOnWebshop { get; set; }
    public string WebshopDescriptionNl { get; set; } = string.Empty;
    public string? EanCode { get; set; }
    public string? PrimaryImageUrl { get; set; }
}

public sealed class ProductListFilter
{
    public string? Search { get; set; }
    public bool? ShowOnWebshop { get; set; }
    public bool ModifiedOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
