using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.VatTypes;

public sealed class VatTypeDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Percentage { get; init; }
    public string InvoiceText { get; init; } = string.Empty;
    public string InvoiceTextEn { get; init; } = string.Empty;
    public string InvoiceTextFr { get; init; } = string.Empty;
    public bool? IsDefault { get; init; }
}

public sealed class VatTypeEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public string InvoiceText { get; set; } = string.Empty;
    public string InvoiceTextEn { get; set; } = string.Empty;
    public string InvoiceTextFr { get; set; } = string.Empty;
    public bool? IsDefault { get; set; }
}

public sealed class VatTypeListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
