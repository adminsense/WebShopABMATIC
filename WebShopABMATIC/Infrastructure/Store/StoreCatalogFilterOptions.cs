namespace WebShopABMATIC.Infrastructure.Store;

/// <summary>
/// Whitelist for Coolblue-style facets on leaf ProductStructure nodes.
/// See docs/PLAN_CATALOG_FILTERS.md — IT/informatics pilot only.
/// </summary>
public sealed class StoreCatalogFilterOptions
{
    public const string SectionName = "StoreCatalogFilters";

    /// <summary>
    /// Leaf <c>ProductStructure</c> ids that show the filter sidebar.
    /// Default: Handzenders (54) — pilot from store filter mock.
    /// Empty list disables facets everywhere unless <see cref="EnableOnAllLeafCategories"/> is true.
    /// </summary>
    public List<int> EnabledCategoryIds { get; set; } = [54];

    /// <summary>When true, every leaf category gets Merk/Prijs/Voorraad (+ ProductProperty when data exists).</summary>
    public bool EnableOnAllLeafCategories { get; set; }

    /// <summary>
    /// Optional: categoryId → ordered ProductProperty ids to show.
    /// When omitted for a category, all properties with values in that leaf are shown (sorted by property SortOrder).
    /// </summary>
    public Dictionary<string, List<int>> CategoryPropertyGroups { get; set; } = new();

    public bool IsEnabledForCategory(int categoryId)
    {
        if (categoryId <= 0)
        {
            return false;
        }

        if (EnableOnAllLeafCategories)
        {
            return true;
        }

        return EnabledCategoryIds.Contains(categoryId);
    }

    public IReadOnlyList<int>? GetPropertyOrder(int categoryId)
    {
        if (CategoryPropertyGroups.TryGetValue(categoryId.ToString(), out var ids) && ids.Count > 0)
        {
            return ids;
        }

        return null;
    }
}
