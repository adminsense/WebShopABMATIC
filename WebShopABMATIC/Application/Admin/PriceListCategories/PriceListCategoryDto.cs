using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.PriceListCategories;

public sealed class PriceListCategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public string? Color { get; init; }
    public bool? HasOptions { get; init; }
}

public sealed class PriceListCategoryEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string? Color { get; set; }
    public bool? HasOptions { get; set; }
}

public sealed class PriceListCategoryListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
