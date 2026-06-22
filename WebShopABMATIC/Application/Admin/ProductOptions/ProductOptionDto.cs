using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.ProductOptions;

public sealed class ProductOptionDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public string NameEn { get; init; } = string.Empty;
    public string ValueType { get; init; } = string.Empty;
    public string Tag { get; init; } = string.Empty;
    public bool IsRequired { get; init; }
    public bool CalculatePrice { get; init; }
    public int SortOrder { get; init; }
}

public sealed class ProductOptionEditDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public bool CalculatePrice { get; set; }
    public int SortOrder { get; set; }
}

public sealed class ProductOptionListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
