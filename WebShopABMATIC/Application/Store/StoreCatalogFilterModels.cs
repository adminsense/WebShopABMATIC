namespace WebShopABMATIC.Application.Store;

/// <summary>Active facet selections for a leaf category product list (Coolblue-style).</summary>
public sealed class StoreCatalogFilterState
{
    /// <summary>Selected manufacturer ids. Use <see cref="IncludeUnknownManufacturer"/> for id 0 / missing.</summary>
    public IReadOnlyList<int> ManufacturerIds { get; init; } = [];

    /// <summary>Include products with no manufacturer (id 0 / missing join).</summary>
    public bool IncludeUnknownManufacturer { get; init; }

    /// <summary>Stock facet keys: <c>in</c>, <c>out</c>.</summary>
    public IReadOnlyList<string> StockKeys { get; init; } = [];

    /// <summary>Price facet keys: <c>request</c>, <c>priced</c>.</summary>
    public IReadOnlyList<string> PriceKeys { get; init; } = [];

    /// <summary>ProductPropertyId → selected free-text values.</summary>
    public IReadOnlyDictionary<int, IReadOnlyList<string>> PropertyValues { get; init; }
        = new Dictionary<int, IReadOnlyList<string>>();

    public bool HasAny =>
        ManufacturerIds.Count > 0
        || IncludeUnknownManufacturer
        || StockKeys.Count > 0
        || PriceKeys.Count > 0
        || PropertyValues.Values.Any(v => v.Count > 0);
}

public sealed class StoreCategoryFacetsDto
{
    public bool Enabled { get; init; }

    /// <summary>Guest hit a facet-enabled leaf category — UI must redirect to customer sign-in.</summary>
    public bool RequiresCustomerLogin { get; init; }

    public int MatchCount { get; init; }
    public IReadOnlyList<StoreFacetGroupDto> Groups { get; init; } = [];
}

public sealed class StoreFacetGroupDto
{
    /// <summary><c>manufacturer</c>, <c>stock</c>, <c>price</c>, or <c>property:{id}</c>.</summary>
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
