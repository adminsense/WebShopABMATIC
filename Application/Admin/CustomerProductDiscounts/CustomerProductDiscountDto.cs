using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.CustomerProductDiscounts;

public sealed class CustomerProductDiscountDto
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public int ProductId { get; init; }
    public decimal? DiscountPercentage { get; init; }
    public DateTime FromAddress { get; init; }
    public DateTime? ValidTo { get; init; }
    public string? Notes { get; init; }
}

public sealed class CustomerProductDiscountEditDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? Notes { get; set; }
}

public sealed class CustomerProductDiscountListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
