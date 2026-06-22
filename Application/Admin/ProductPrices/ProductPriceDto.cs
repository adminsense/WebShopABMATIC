using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.ProductPrices;

public sealed class ProductPriceDto
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public DateTime FromAddress { get; init; }
    public DateTime? ValidTo { get; init; }
    public decimal GrossSalesPrice { get; init; }
    public decimal GrossPurchasePrice { get; init; }
    public decimal NetPurchasePrice { get; init; }
    public decimal BasePrice { get; init; }
    public decimal AssemblyPrice { get; init; }
    public decimal InstallationPrice { get; init; }
}

public sealed class ProductPriceEditDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal GrossSalesPrice { get; set; }
    public decimal GrossPurchasePrice { get; set; }
    public decimal NetPurchasePrice { get; set; }
    public decimal BasePrice { get; set; }
    public decimal AssemblyPrice { get; set; }
    public decimal InstallationPrice { get; set; }
}

public sealed class ProductPriceListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
