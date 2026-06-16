using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.CustomerTypes;

public sealed class CustomerTypeDto
{
    public int KlantTypeId { get; init; }
    public string CustomerTypeName { get; init; } = string.Empty;
    public string CustomerTypeNameFr { get; init; } = string.Empty;
    public decimal BaseDiscount { get; init; }
    public int SortOrder { get; init; }
    public int PaymentTermId { get; init; }
    public int VatSystemId { get; init; }
    public int DeliveryTypeId { get; init; }
    public bool RequiresVatNumber { get; init; }
    public bool? IsDefault { get; init; }
}

public sealed class CustomerTypeEditDto
{
    public int KlantTypeId { get; set; }
    public string CustomerTypeName { get; set; } = string.Empty;
    public string CustomerTypeNameFr { get; set; } = string.Empty;
    public decimal BaseDiscount { get; set; }
    public int SortOrder { get; set; }
    public int PaymentTermId { get; set; }
    public int VatSystemId { get; set; }
    public int DeliveryTypeId { get; set; }
    public bool RequiresVatNumber { get; set; }
    public bool? IsDefault { get; set; }
}

public sealed class CustomerTypeListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
