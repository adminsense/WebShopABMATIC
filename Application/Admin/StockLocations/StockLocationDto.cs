namespace WebShopABMATIC.Application.Admin.StockLocations;

public sealed class StockLocationDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool? IsWarehouse { get; init; }
}

public sealed class StockLocationEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsWarehouse { get; set; }
}

public sealed class StockLocationListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
