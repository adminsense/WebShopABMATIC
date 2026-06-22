using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.Customers;

public sealed class CustomerDto
{
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string? WebshopLogin { get; init; }
    public int CustomerTypeId { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public int CustomerCityId { get; init; }
    public bool Locked { get; init; }
    public bool HasWebshopAccount { get; init; }
}

public sealed class CustomerEditDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? WebshopLogin { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int CustomerTypeId { get; set; }
    public int CustomerCityId { get; set; }
    public string CustomerStreet { get; set; } = string.Empty;
    public string CustomerHouseNumber { get; set; } = string.Empty;
    public string CustomerBox { get; set; } = string.Empty;
    public string CustomerVatNumber { get; set; } = string.Empty;
    public int DeliveryTypeId { get; set; }
    public int BetaaltermijnId { get; set; }
    public bool Locked { get; set; }
    public bool HasWebshopAccount { get; set; }
}

public sealed class CustomerListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
