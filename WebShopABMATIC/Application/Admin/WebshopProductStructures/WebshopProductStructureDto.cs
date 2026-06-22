using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.WebshopProductStructures;

public sealed class WebshopProductStructureDto
{
    public int Id { get; init; }
    public string NameEn { get; init; } = string.Empty;
    public string NameNl { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public int? ParentTaskId { get; init; }
}

public sealed class WebshopProductStructureEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameNl { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public int? ParentTaskId { get; set; }
}

public sealed class WebshopProductStructureListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
