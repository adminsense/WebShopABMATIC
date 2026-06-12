namespace WebShopABMATIC.Application.Admin.Suppliers;

public sealed class SupplierDto
{
    public int SupplierId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? CityId { get; init; }
    public int LanguageId { get; init; }
    public string? Email { get; init; }
    public int GeneralLedgerRevenueAccount { get; init; }
    public bool IsInactive { get; init; }
}

public sealed class SupplierEditDto
{
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public int LanguageId { get; set; }
    public string? Email { get; set; }
    public int GeneralLedgerRevenueAccount { get; set; }
    public bool IsInactive { get; set; }
}

public sealed class SupplierListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
