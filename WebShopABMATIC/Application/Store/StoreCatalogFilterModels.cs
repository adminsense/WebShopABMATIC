namespace WebShopABMATIC.Application.Store;

/// <summary>Active ProductAttribuut facet selections for a leaf category product list.</summary>
public sealed class StoreCatalogFilterState
{
    /// <summary>ProductAttributeId → selected Waarde values (catalog filters).</summary>
    public IReadOnlyDictionary<int, IReadOnlyList<string>> AttributeValues { get; init; }
        = new Dictionary<int, IReadOnlyList<string>>();

    public bool HasAny =>
        AttributeValues.Values.Any(v => v.Count > 0);
}

public sealed class StoreCategoryFacetsDto
{
    public bool Enabled { get; init; }
    public int MatchCount { get; init; }
    public IReadOnlyList<StoreFacetGroupDto> Groups { get; init; } = [];
}

public sealed class StoreFacetGroupDto
{
    /// <summary><c>attr:{ProductAttributeId}</c>.</summary>
    public string Key { get; init; } = "";
    public string Title { get; init; } = "";
    public bool IsMuted { get; init; }
    public string? Note { get; init; }
    public IReadOnlyList<StoreFacetValueDto> Values { get; init; } = [];
}

public sealed class StoreFacetValueDto
{
    public string Value { get; init; } = "";
    public string Label { get; init; } = "";
    public int Count { get; init; }
    public bool Selected { get; init; }
}
