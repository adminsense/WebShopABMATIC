using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.WebshopStructures;

public sealed class WebshopStructureDto
{
    public int Id { get; init; }
    public string NameNl { get; init; } = string.Empty;
    public int? ParentTaskId { get; init; }
    public int SortOrder { get; init; }
}

public sealed class WebshopStructureEditDto
{
    public int Id { get; set; }
    public string NameNl { get; set; } = string.Empty;
    public int? ParentTaskId { get; set; }
    public int SortOrder { get; set; }
}

public sealed class WebshopStructureListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
