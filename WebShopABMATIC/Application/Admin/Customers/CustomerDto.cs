namespace WebShopABMATIC.Application.Admin.Customers;

public sealed class CustomerDto
{
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string? WebshopLogin { get; init; }
    public int CustomerTypeId { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
}

public sealed class CustomerListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
