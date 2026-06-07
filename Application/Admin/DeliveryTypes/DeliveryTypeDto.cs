namespace WebShopABMATIC.Application.Admin.DeliveryTypes;

public sealed class DeliveryTypeDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public bool IncludeInstallationCost { get; init; }
    public bool? IsDefault { get; init; }
}

public sealed class DeliveryTypeEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public bool IncludeInstallationCost { get; set; }
    public bool? IsDefault { get; set; }
}

public sealed class DeliveryTypeListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
