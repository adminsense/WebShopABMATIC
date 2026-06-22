using WebShopABMATIC.Data.Entities;

namespace WebShopABMATIC.Infrastructure.Store;

internal static class CatalogCategoryTree
{
    public static int ResolveRootId(int structureId, IReadOnlyDictionary<int, ProductStructure> structures)
    {
        var current = structureId;
        var visited = new HashSet<int>();
        while (structures.TryGetValue(current, out var node)
               && node.ParentTaskId is int parent
               && parent > 0
               && visited.Add(current))
        {
            current = parent;
        }

        return current;
    }

    public static HashSet<int> CollectDescendantIds(int rootId, IReadOnlyDictionary<int, ProductStructure> structures)
    {
        var result = new HashSet<int> { rootId };
        foreach (var child in structures.Values.Where(s => s.ParentTaskId == rootId))
        {
            result.UnionWith(CollectDescendantIds(child.Id, structures));
        }

        return result;
    }

    public static string PickDisplayName(ProductStructure structure) =>
        !string.IsNullOrWhiteSpace(structure.NameEn) ? structure.NameEn
        : !string.IsNullOrWhiteSpace(structure.NameNl) ? structure.NameNl
        : structure.NameFr;
}
