namespace WebShopABMATIC.Application.Store;

public sealed class StoreCategoryTreeNodeDto
{
    public int Id { get; init; }
    public int? ParentId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }
    public int? ProductCount { get; init; }
    public bool HasIcon { get; init; }
    public IReadOnlyList<StoreCategoryTreeNodeDto> Children { get; init; } = [];
}
