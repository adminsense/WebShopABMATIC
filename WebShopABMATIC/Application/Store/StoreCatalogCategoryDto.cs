namespace WebShopABMATIC.Application.Store;

public sealed class StoreCatalogCategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int ProductCount { get; init; }
}
