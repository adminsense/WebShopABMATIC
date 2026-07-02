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

    public static int? NormalizeParentId(int? parentTaskId) =>
        parentTaskId is null or 0 ? null : parentTaskId;

    /// <summary>
    /// Top-level node: no parent, or parent id missing from the loaded structure set (orphan / legacy link).
    /// </summary>
    public static bool IsStructuralRoot(ProductStructure structure, IReadOnlyDictionary<int, ProductStructure> structures)
    {
        var parent = NormalizeParentId(structure.ParentTaskId);
        if (parent is null)
        {
            return true;
        }

        return !structures.ContainsKey(parent.Value);
    }

    public static HashSet<int> CollectDescendantIds(int rootId, IReadOnlyDictionary<int, ProductStructure> structures)
    {
        var result = new HashSet<int> { rootId };
        foreach (var child in structures.Values.Where(s => NormalizeParentId(s.ParentTaskId) == rootId))
        {
            result.UnionWith(CollectDescendantIds(child.Id, structures));
        }

        return result;
    }

    /// <summary>True when the node has at least one child in the loaded structure set (CD4 — not a leaf).</summary>
    public static bool HasStructuralChildren(int structureId, IReadOnlyDictionary<int, ProductStructure> structures) =>
        structures.Values.Any(s => NormalizeParentId(s.ParentTaskId) == structureId);

    public static string PickDisplayName(ProductStructure structure) =>
        !string.IsNullOrWhiteSpace(structure.NameEn) ? structure.NameEn
        : !string.IsNullOrWhiteSpace(structure.NameNl) ? structure.NameNl
        : structure.NameFr;
}
