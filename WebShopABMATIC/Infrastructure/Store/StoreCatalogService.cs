using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Store;

public sealed class StoreCatalogService : IStoreCatalogPort
{
    private const string ProductStructuresCacheKey = "store:product-structures";
    private const string CategoryTreeCacheKey = "store:category-tree";
    private static readonly TimeSpan CategoryTreeCacheDuration = TimeSpan.FromMinutes(2);

    private readonly WebShopABMATICDbContext _db;
    private readonly IProductMediaPort _media;
    private readonly IProductPricingPort _pricing;
    private readonly IMemoryCache _cache;
    private readonly ILogger<StoreCatalogService> _logger;

    // Blazor Server shares one scoped DbContext per circuit. The sidebar and the
    // catalog page both query it, so concurrent access throws. Serialize all DB work.
    private readonly SemaphoreSlim _dbGate = new(1, 1);

    private async Task<T> RunSerializedAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        await _dbGate.WaitAsync(cancellationToken);
        try
        {
            return await operation();
        }
        finally
        {
            _dbGate.Release();
        }
    }

    public StoreCatalogService(
        WebShopABMATICDbContext db,
        IProductMediaPort media,
        IProductPricingPort pricing,
        IMemoryCache cache,
        ILogger<StoreCatalogService> logger)
    {
        _db = db;
        _media = media;
        _pricing = pricing;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<StoreCatalogCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var tree = await GetCategoryTreeAsync(cancellationToken);
            return tree
                .Select(n => new StoreCatalogCategoryDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    ProductCount = n.ProductCount ?? 0
                })
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    public async Task<IReadOnlyList<StoreCategoryTreeNodeDto>> GetCategoryTreeAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(CategoryTreeCacheKey, out IReadOnlyList<StoreCategoryTreeNodeDto>? cachedTree)
            && cachedTree is not null)
        {
            return cachedTree;
        }

        return await RunSerializedAsync(() => GetCategoryTreeCoreAsync(cancellationToken), cancellationToken);
    }

    private async Task<IReadOnlyList<StoreCategoryTreeNodeDto>> GetCategoryTreeCoreAsync(CancellationToken cancellationToken)
    {
        try
        {
            var webshopNav = await LoadWebshopNavTreeAsync(cancellationToken);
            if (webshopNav.Count > 0)
            {
                _cache.Set(CategoryTreeCacheKey, webshopNav, CategoryTreeCacheDuration);
                return webshopNav;
            }

            var structures = await LoadProductStructuresAsync(cancellationToken);
            if (structures.Count == 0)
            {
                return [];
            }

            var productStructureIds = await _db.Products.AsNoTracking()
                .Where(p => (p.ShowOnWebshop ?? false) && !p.IsInactive && p.ProductStructureId != null)
                .Select(p => p.ProductStructureId!.Value)
                .ToListAsync(cancellationToken);

            var counts = BuildProductCounts(structures, productStructureIds);
            var tree = BuildTreeNodes(structures, counts, parentId: null);

            if (tree.Count == 0 && productStructureIds.Count > 0)
            {
                tree = BuildTreeFromProductRoots(structures, counts, productStructureIds);
            }

            if (tree.Count == 0 && productStructureIds.Count > 0)
            {
                tree = BuildFlatRootsFromProducts(structures, productStructureIds);
            }

            _cache.Set(CategoryTreeCacheKey, tree, CategoryTreeCacheDuration);
            return tree;
        }
        catch
        {
            return [];
        }
    }

    public Task<IReadOnlyList<StoreProductDto>> GetNewProductsAsync(int take, CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetNewProductsCoreAsync(take, cancellationToken), cancellationToken);

    private async Task<IReadOnlyList<StoreProductDto>> GetNewProductsCoreAsync(int take, CancellationToken cancellationToken)
    {
        var safeTake = take > 0 ? take : 12;
        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);
            var products = await LoadProductRowsAsync(
                QueryVisibleProducts().Where(p => p.IsNew == true),
                safeTake,
                cancellationToken);

            return await MapProductRowsAsync(products, structures, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load frontpage products (IsNieuw).");
            return [];
        }
    }

    public Task<IReadOnlyList<StoreProductDto>> GetDealsAsync(int take, CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetNewProductsCoreAsync(take > 0 ? take : 8, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<StoreProductDto>> GetCatalogAsync(
        int? take = null,
        int? categoryId = null,
        CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetCatalogCoreAsync(take, categoryId, cancellationToken), cancellationToken);

    private async Task<IReadOnlyList<StoreProductDto>> GetCatalogCoreAsync(
        int? take,
        int? categoryId,
        CancellationToken cancellationToken)
    {
        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);

            List<CatalogProductRow> products;
            if (categoryId is > 0)
            {
                var category = categoryId.Value;
                if (CatalogCategoryTree.HasStructuralChildren(category, structures))
                {
                    // CD4: products only on leaf nodes; browse UI shows child tiles instead.
                    products = [];
                }
                else
                {
                    var rows = await LoadProductRowsAsync(QueryVisibleProducts(), take: null, cancellationToken);
                    products = rows
                        .Where(p => p.ProductStructureId == category)
                        .ToList();
                    if (take is > 0)
                    {
                        products = products.Take(take.Value).ToList();
                    }
                }
            }
            else
            {
                products = await LoadProductRowsAsync(QueryVisibleProducts(), take, cancellationToken);
            }

            return await MapProductRowsAsync(products, structures, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load catalog (categoryId={CategoryId}).", categoryId);
            return [];
        }
    }

    public Task<byte[]?> GetCategoryIconAsync(int categoryId, CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetCategoryIconCoreAsync(categoryId, cancellationToken), cancellationToken);

    private async Task<byte[]?> GetCategoryIconCoreAsync(int categoryId, CancellationToken cancellationToken)
    {
        try
        {
            return await _db.ProductStructures.AsNoTracking()
                .Where(s => s.Id == categoryId)
                .Select(s => s.Icon)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load icon for category {CategoryId}.", categoryId);
            return null;
        }
    }

    public Task<StoreProductDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetByIdCoreAsync(productId, cancellationToken), cancellationToken);

    private async Task<StoreProductDto?> GetByIdCoreAsync(int productId, CancellationToken cancellationToken)
    {
        try
        {
            var p = await _db.Products.AsNoTracking()
                .Where(x => x.ProductId == productId && x.ShowOnWebshop == true && !x.IsInactive)
                .Select(x => new
                {
                    x.ProductId,
                    IsNew = x.IsNew == true,
                    x.NameEn,
                    x.WebshopDescriptionNl,
                    x.ProductStructureId,
                    x.HasNoPrice
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (p is null)
            {
                return null;
            }

            var structures = await LoadProductStructuresAsync(cancellationToken);
            var stockLevels = await GetDefaultStockLevelsAsync([productId], cancellationToken);
            stockLevels.TryGetValue(productId, out var level);

            var price = await _pricing.GetUnitPriceAsync(productId, cancellationToken: cancellationToken);
            var (categoryIdValue, categoryRootId, categoryName) = ResolveCategory(p.ProductStructureId, structures);

            return MapProduct(
                p.ProductId,
                p.IsNew,
                p.NameEn,
                p.WebshopDescriptionNl,
                await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: true, cancellationToken)
                    ?? FallbackImage(productId),
                level.Quantity,
                level.MinQuantity,
                ResolveStorePrice(p.HasNoPrice, price),
                categoryIdValue,
                categoryRootId,
                categoryName);
        }
        catch
        {
            return null;
        }
    }

    private static Dictionary<int, int> BuildProductCounts(
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyList<int> productStructureIds)
    {
        var direct = productStructureIds
            .GroupBy(id => id)
            .ToDictionary(g => g.Key, g => g.Count());

        var counts = new Dictionary<int, int>();
        foreach (var structure in structures.Values)
        {
            var subtree = CatalogCategoryTree.CollectDescendantIds(structure.Id, structures);
            var total = direct.Where(kv => subtree.Contains(kv.Key)).Sum(kv => kv.Value);
            if (total > 0)
            {
                counts[structure.Id] = total;
            }
        }

        return counts;
    }

    private static List<StoreCategoryTreeNodeDto> BuildTreeNodes(
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<int, int> counts,
        int? parentId)
    {
        var candidates = parentId is null
            ? structures.Values.Where(s => CatalogCategoryTree.IsStructuralRoot(s, structures))
            : structures.Values.Where(s => CatalogCategoryTree.NormalizeParentId(s.ParentTaskId) == parentId);

        return candidates
            .Where(s => counts.ContainsKey(s.Id))
            .OrderBy(s => s.SortOrder)
            .ThenBy(s => CatalogCategoryTree.PickDisplayName(s))
            .Select(s => new StoreCategoryTreeNodeDto
            {
                Id = s.Id,
                ParentId = CatalogCategoryTree.NormalizeParentId(s.ParentTaskId),
                Name = CatalogCategoryTree.PickDisplayName(s),
                Level = s.Level,
                ProductCount = counts[s.Id],
                HasIcon = HasIcon(s),
                Children = BuildTreeNodes(structures, counts, s.Id)
            })
            .ToList();
    }

    private static bool HasIcon(ProductStructure structure) =>
        structure.Icon is { Length: > 0 };

    private static List<StoreCategoryTreeNodeDto> BuildTreeFromProductRoots(
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<int, int> counts,
        IReadOnlyList<int> productStructureIds)
    {
        var rootIds = productStructureIds
            .Select(id => CatalogCategoryTree.ResolveRootId(id, structures))
            .Where(id => structures.ContainsKey(id) && counts.ContainsKey(id))
            .Distinct()
            .OrderBy(id => structures[id].SortOrder)
            .ThenBy(id => CatalogCategoryTree.PickDisplayName(structures[id]))
            .ToList();

        return rootIds
            .Select(id =>
            {
                var s = structures[id];
                return new StoreCategoryTreeNodeDto
                {
                    Id = s.Id,
                    ParentId = CatalogCategoryTree.NormalizeParentId(s.ParentTaskId),
                    Name = CatalogCategoryTree.PickDisplayName(s),
                    Level = s.Level,
                    ProductCount = counts[id],
                    HasIcon = HasIcon(s),
                    Children = BuildTreeNodes(structures, counts, s.Id)
                };
            })
            .ToList();
    }

    private static List<StoreCategoryTreeNodeDto> BuildFlatRootsFromProducts(
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyList<int> productStructureIds)
    {
        var rootCounts = new Dictionary<int, int>();
        foreach (var structureId in productStructureIds)
        {
            if (!structures.ContainsKey(structureId))
            {
                continue;
            }

            var rootId = CatalogCategoryTree.ResolveRootId(structureId, structures);
            if (!structures.ContainsKey(rootId))
            {
                continue;
            }

            rootCounts[rootId] = rootCounts.GetValueOrDefault(rootId) + 1;
        }

        return rootCounts
            .Select(kv =>
            {
                var s = structures[kv.Key];
                return new StoreCategoryTreeNodeDto
                {
                    Id = s.Id,
                    ParentId = CatalogCategoryTree.NormalizeParentId(s.ParentTaskId),
                    Name = CatalogCategoryTree.PickDisplayName(s),
                    Level = s.Level,
                    ProductCount = kv.Value,
                    HasIcon = HasIcon(s),
                    Children = []
                };
            })
            .OrderBy(n => structures[n.Id].SortOrder)
            .ThenBy(n => n.Name)
            .ToList();
    }

    private async Task<List<StoreCategoryTreeNodeDto>> LoadWebshopNavTreeAsync(CancellationToken cancellationToken)
    {
        var rows = await _db.WebshopStructures.AsNoTracking()
            .OrderBy(s => s.SortOrder)
            .ThenBy(s => s.NameNl)
            .ToListAsync(cancellationToken);

        if (rows.Count == 0)
        {
            return [];
        }

        var byId = rows.ToDictionary(s => s.Id);
        return rows
            .Where(s => IsWebshopStructuralRoot(s, byId))
            .Select(s => new StoreCategoryTreeNodeDto
            {
                Id = s.Id,
                ParentId = CatalogCategoryTree.NormalizeParentId(s.ParentTaskId),
                Name = s.NameNl,
                ProductCount = 0,
                Children = BuildWebshopNavChildren(rows, s.Id)
            })
            .ToList();
    }

    private static bool IsWebshopStructuralRoot(
        WebshopStructure structure,
        IReadOnlyDictionary<int, WebshopStructure> structures)
    {
        var parent = CatalogCategoryTree.NormalizeParentId(structure.ParentTaskId);
        if (parent is null)
        {
            return true;
        }

        return !structures.ContainsKey(parent.Value);
    }

    private static List<StoreCategoryTreeNodeDto> BuildWebshopNavChildren(
        IReadOnlyList<WebshopStructure> rows,
        int parentId) =>
        rows
            .Where(s => CatalogCategoryTree.NormalizeParentId(s.ParentTaskId) == parentId)
            .Select(s => new StoreCategoryTreeNodeDto
            {
                Id = s.Id,
                ParentId = CatalogCategoryTree.NormalizeParentId(s.ParentTaskId),
                Name = s.NameNl,
                ProductCount = 0,
                Children = BuildWebshopNavChildren(rows, s.Id)
            })
            .ToList();
    private IQueryable<Product> QueryVisibleProducts() =>
        _db.Products.AsNoTracking()
            .Where(p => (p.ShowOnWebshop ?? false) && !p.IsInactive);

    private static async Task<List<CatalogProductRow>> LoadProductRowsAsync(
        IQueryable<Product> query,
        int? take,
        CancellationToken cancellationToken)
    {
        var ordered = query.OrderBy(p => p.NameEn);
        var limited = take is > 0 ? ordered.Take(take.Value) : ordered;

        var rows = await limited
            .Select(p => new
            {
                p.ProductId,
                IsNew = p.IsNew == true,
                p.NameEn,
                p.DescriptionEn,
                p.ProductStructureId,
                p.HasNoPrice
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(p => new CatalogProductRow(
                p.ProductId,
                p.IsNew,
                p.NameEn ?? string.Empty,
                p.DescriptionEn ?? string.Empty,
                p.ProductStructureId,
                p.HasNoPrice))
            .ToList();
    }

    private async Task<IReadOnlyDictionary<int, decimal>> SafeGetCatalogPricesAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _pricing.GetCatalogPricesAsync(productIds, cancellationToken: cancellationToken);
        }
        catch
        {
            return new Dictionary<int, decimal>();
        }
    }

    private async Task<IReadOnlyDictionary<int, string>> SafeGetPrimaryImageUrlsAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _media.GetPrimaryImageUrlsAsync(productIds, webPublishedOnly: true, cancellationToken);
        }
        catch
        {
            return new Dictionary<int, string>();
        }
    }

    private async Task<Dictionary<int, (int Quantity, decimal MinQuantity)>> SafeGetDefaultStockLevelsAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetDefaultStockLevelsAsync(productIds, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load stock levels for {Count} products.", productIds.Count);
            return new Dictionary<int, (int, decimal)>();
        }
    }

    private async Task<IReadOnlyList<StoreProductDto>> MapProductRowsAsync(
        IReadOnlyList<CatalogProductRow> products,
        IReadOnlyDictionary<int, ProductStructure> structures,
        CancellationToken cancellationToken)
    {
        if (products.Count == 0)
        {
            return [];
        }

        var productIds = products.Select(p => p.ProductId).ToList();
        var prices = await SafeGetCatalogPricesAsync(productIds, cancellationToken);
        var stockLevels = await SafeGetDefaultStockLevelsAsync(productIds, cancellationToken);
        var imageUrls = await SafeGetPrimaryImageUrlsAsync(productIds, cancellationToken);

        var items = new List<StoreProductDto>();
        foreach (var p in products)
        {
            stockLevels.TryGetValue(p.ProductId, out var level);
            var (categoryIdValue, categoryRootId, categoryName) = ResolveCategory(p.ProductStructureId, structures);

            items.Add(MapProduct(
                p.ProductId,
                p.IsNew,
                p.NameEn,
                p.WebshopDescriptionNl,
                imageUrls.GetValueOrDefault(p.ProductId) ?? FallbackImage(p.ProductId),
                level.Quantity,
                level.MinQuantity,
                ResolveStorePrice(p.HasNoPrice, prices, p.ProductId),
                categoryIdValue,
                categoryRootId,
                categoryName));
        }

        return items;
    }

    private async Task<Dictionary<int, ProductStructure>> LoadProductStructuresAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(ProductStructuresCacheKey, out Dictionary<int, ProductStructure>? cached)
            && cached is not null)
        {
            return cached;
        }

        var rows = await _db.ProductStructures.AsNoTracking().ToListAsync(cancellationToken);
        var structures = rows.ToDictionary(s => s.Id);
        _cache.Set(ProductStructuresCacheKey, structures, TimeSpan.FromMinutes(10));
        return structures;
    }

    private static (int? CategoryId, int? CategoryRootId, string CategoryName) ResolveCategory(
        int? structureId,
        IReadOnlyDictionary<int, ProductStructure> structures)
    {
        if (structureId is not int id || !structures.TryGetValue(id, out var structure))
        {
            return (null, null, string.Empty);
        }

        var rootId = CatalogCategoryTree.ResolveRootId(id, structures);
        return (id, rootId, CatalogCategoryTree.PickDisplayName(structure));
    }

    private async Task<Dictionary<int, (int Quantity, decimal MinQuantity)>> GetDefaultStockLevelsAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<int, (int, decimal)>();
        }

        var rows = await _db.ProductStockLocations.AsNoTracking()
            .Where(x => x.IsDefault && productIds.Contains(x.ProductId))
            .Select(x => new { x.ProductId, x.Quantity, x.ReservedQuantity, x.MinQuantity })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(
            x => x.ProductId,
            x => ((int)Math.Max(0, x.Quantity - x.ReservedQuantity), x.MinQuantity));
    }

    private static decimal? ResolveStorePrice(
        bool hasNoPrice,
        IReadOnlyDictionary<int, decimal> prices,
        int productId)
    {
        if (hasNoPrice || !prices.TryGetValue(productId, out var price) || price <= 0)
        {
            return null;
        }

        return price;
    }

    private static decimal? ResolveStorePrice(bool hasNoPrice, decimal? price) =>
        hasNoPrice || price is null or <= 0 ? null : price;

    private static string FallbackImage(int productId) =>
        productId is >= 1 and <= 6 ? $"/images/product{productId}.png" : "/images/product1.png";

    private sealed record CatalogProductRow(
        int ProductId,
        bool IsNew,
        string NameEn,
        string WebshopDescriptionNl,
        int? ProductStructureId,
        bool HasNoPrice);

    private static StoreProductDto MapProduct(
        int id,
        bool isNew,
        string name,
        string description,
        string imageUrl,
        int qty,
        decimal minQuantity,
        decimal? price,
        int? categoryId,
        int? categoryRootId,
        string categoryName) =>
        new()
        {
            Id = id,
            IsNew = isNew,
            Name = name,
            Description = description,
            ImageUrl = imageUrl,
            Price = price,
            Stock = qty,
            MinQuantity = minQuantity,
            CategoryId = categoryId,
            CategoryRootId = categoryRootId,
            CategoryName = categoryName
        };
}
