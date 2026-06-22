using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.OrderStatuses;

public sealed class OrderStatusDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public string ScreenMode { get; init; } = string.Empty;
    public int? SortOrder { get; init; }
    public bool ReserveStock { get; init; }
    public bool AffectsStock { get; init; }
}

public sealed class OrderStatusEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public string ScreenMode { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public bool ReserveStock { get; set; }
    public bool AffectsStock { get; set; }
}

public sealed class OrderStatusListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
