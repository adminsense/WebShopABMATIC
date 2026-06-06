namespace WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;

public sealed class CustomerDeliveryAddressDto
{
    public int Id { get; init; }
    public int? CustomerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Straat { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
    public string Bus { get; init; } = string.Empty;
    public int CityId { get; init; }
}

public sealed class CustomerDeliveryAddressEditDto
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Straat { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Bus { get; set; } = string.Empty;
    public int CityId { get; set; }
}

public sealed class CustomerDeliveryAddressListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
