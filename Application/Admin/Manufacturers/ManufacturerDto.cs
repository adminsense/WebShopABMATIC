namespace WebShopABMATIC.Application.Admin.Manufacturers;

public sealed class ManufacturerDto
{
    public int ManufacturerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int? CityId { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
}

public sealed class ManufacturerEditDto
{
    public int ManufacturerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public sealed class ManufacturerListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
