using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Store;

public sealed class StoreCatalogService : IStoreCatalogPort
{
    private const string ProductStructuresCacheKey = "store:product-structures:meta:v2";
    private const string StructureHasIconCacheKey = "store:structure-has-icon:v2";
    private const string CategoryIconsCacheKey = "store:category-icons"; // unused bulk; per-id keys used instead
    private const string CategoryTreeCacheKey = "store:category-tree:v7";
    private static readonly TimeSpan CategoryTreeCacheDuration = TimeSpan.FromMinutes(5);

    private readonly WebShopABMATICDbContext _db;
    private readonly IProductMediaPort _media;
    private readonly IProductPricingPort _pricing;
    private readonly IMemoryCache _cache;
    private readonly ILogger<StoreCatalogService> _logger;
    private readonly StoreDbGate _dbGate;
    private readonly StoreCatalogFilterOptions _filterOptions;
    private readonly ICurrentUserContext _currentUser;

    private Task<T> RunSerializedAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken) =>
        _dbGate.RunAsync(operation, cancellationToken);

    public StoreCatalogService(
        WebShopABMATICDbContext db,
        IProductMediaPort media,
        IProductPricingPort pricing,
        IMemoryCache cache,
        ILogger<StoreCatalogService> logger,
        StoreDbGate dbGate,
        IOptions<StoreCatalogFilterOptions> filterOptions,
        ICurrentUserContext currentUser)
    {
        _db = db;
        _media = media;
        _pricing = pricing;
        _cache = cache;
        _logger = logger;
        _dbGate = dbGate;
        _filterOptions = filterOptions.Value;
        _currentUser = currentUser;
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
            var structures = await LoadProductStructuresAsync(cancellationToken);
            var hasIcon = await LoadStructureHasIconFlagsAsync(cancellationToken);
            var productStructureIds = await _db.Products.AsNoTracking()
                .Where(p => (p.ShowOnWebshop ?? false) && !p.IsInactive
                            && p.ProductStructureId != null)
                .Select(p => p.ProductStructureId!.Value)
                .ToListAsync(cancellationToken);
            var counts = structures.Count > 0
                ? BuildProductCounts(structures, productStructureIds)
                : new Dictionary<int, int>();

            var webshopNav = await LoadWebshopNavTreeAsync(structures, counts, hasIcon, cancellationToken);
            if (webshopNav.Count > 0)
            {
                var pruned = PruneEmptyCategories(webshopNav);
                // If counts fail to map (legacy id mismatch), never ship an empty menu.
                var nav = pruned.Count > 0 ? pruned : webshopNav;
                _cache.Set(CategoryTreeCacheKey, nav, CategoryTreeCacheDuration);
                return nav;
            }

            if (structures.Count == 0)
            {
                return [];
            }

            var structureTree = BuildProductStructureNavTree(structures, counts, hasIcon, parentId: null);
            var prunedStructure = PruneEmptyCategories(structureTree);
            var tree = prunedStructure.Count > 0 ? prunedStructure : structureTree;
            _cache.Set(CategoryTreeCacheKey, tree, CategoryTreeCacheDuration);
            return tree;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load storefront category tree.");
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
                QueryVisibleLinkedProducts().Where(p => p.IsNew == true),
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
        StoreCatalogFilterState? filters = null,
        CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetCatalogCoreAsync(take, categoryId, filters, cancellationToken), cancellationToken);

    public async Task<StoreCategoryFacetsDto> GetCategoryFacetsAsync(
        int categoryId,
        StoreCatalogFilterState? filters = null,
        CancellationToken cancellationToken = default)
    {
        var result = await RunSerializedAsync(
            () => GetCategoryFacetsCoreAsync(categoryId, filters, cancellationToken),
            cancellationToken);
        return result ?? new StoreCategoryFacetsDto { Enabled = false };
    }

    private async Task<IReadOnlyList<StoreProductDto>> GetCatalogCoreAsync(
        int? take,
        int? categoryId,
        StoreCatalogFilterState? filters,
        CancellationToken cancellationToken)
    {
        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);

            List<CatalogProductRow> products;
            if (categoryId is > 0)
            {
                var category = categoryId.Value;
                if (_filterOptions.IsEnabledForCategory(category)
                    && !CatalogCategoryTree.HasStructuralChildren(category, structures)
                    && !await IsAuthenticatedCustomerAsync(cancellationToken))
                {
                    return [];
                }

                if (CatalogCategoryTree.HasStructuralChildren(category, structures))
                {
                    // CD4: products only on leaf nodes; browse UI shows child tiles instead.
                    products = [];
                }
                else
                {
                    // Leaf only — never pull the whole catalog then filter in memory.
                    products = await LoadProductRowsAsync(
                        QueryVisibleLinkedProducts().Where(p => p.ProductStructureId == category),
                        take: null,
                        cancellationToken);
                }
            }
            else
            {
                // Full catalog is intentionally limited: search must use SearchProductsAsync.
                products = await LoadProductRowsAsync(
                    QueryVisibleLinkedProducts(),
                    take ?? 48,
                    cancellationToken);
            }

            var mapped = await MapProductRowsAsync(products, structures, cancellationToken);
            if (categoryId is > 0
                && _filterOptions.IsEnabledForCategory(categoryId.Value)
                && filters is { HasAny: true })
            {
                var propertyMap = await LoadPropertyValuesAsync(
                    mapped.Select(p => p.Id).ToList(),
                    cancellationToken);
                mapped = ApplyFilters(mapped, filters, propertyMap);
            }

            if (take is > 0 && mapped.Count > take.Value)
            {
                mapped = mapped.Take(take.Value).ToList();
            }

            return mapped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load catalog (categoryId={CategoryId}).", categoryId);
            return [];
        }
    }

    private async Task<StoreCategoryFacetsDto> GetCategoryFacetsCoreAsync(
        int categoryId,
        StoreCatalogFilterState? filters,
        CancellationToken cancellationToken)
    {
        if (!_filterOptions.IsEnabledForCategory(categoryId))
        {
            return new StoreCategoryFacetsDto { Enabled = false };
        }

        if (!await IsAuthenticatedCustomerAsync(cancellationToken))
        {
            return new StoreCategoryFacetsDto { Enabled = false, RequiresCustomerLogin = true };
        }

        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);
            if (CatalogCategoryTree.HasStructuralChildren(categoryId, structures))
            {
                return new StoreCategoryFacetsDto { Enabled = false };
            }

            var rows = await LoadProductRowsAsync(
                QueryVisibleLinkedProducts().Where(p => p.ProductStructureId == categoryId),
                take: null,
                cancellationToken);
            var products = await MapProductRowsAsync(rows, structures, cancellationToken);
            var propertyMap = await LoadPropertyValuesAsync(
                products.Select(p => p.Id).ToList(),
                cancellationToken);
            var active = filters ?? new StoreCatalogFilterState();
            var matched = ApplyFilters(products, active, propertyMap);
            var groups = BuildFacetGroups(products, matched, active, propertyMap, categoryId);

            return new StoreCategoryFacetsDto
            {
                Enabled = true,
                MatchCount = matched.Count,
                Groups = groups
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load category facets (categoryId={CategoryId}).", categoryId);
            return new StoreCategoryFacetsDto { Enabled = false };
        }
    }

    private async Task<bool> IsAuthenticatedCustomerAsync(CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUserAsync(cancellationToken);
        return user.CustomerId is > 0;
    }

    public Task<IReadOnlyList<StoreProductDto>> SearchProductsAsync(
        string term,
        int take = 24,
        CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => SearchProductsCoreAsync(term, take, cancellationToken), cancellationToken);

    private async Task<IReadOnlyList<StoreProductDto>> SearchProductsCoreAsync(
        string term,
        int take,
        CancellationToken cancellationToken)
    {
        var q = term.Trim();
        if (q.Length == 0)
        {
            return [];
        }

        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);
            var safeTake = take > 0 ? take : 24;
            var products = await LoadProductRowsAsync(
                QueryVisibleLinkedProducts()
                    .Where(p => p.NameEn != null && p.NameEn.StartsWith(q)),
                safeTake,
                cancellationToken);
            return await MapProductRowsAsync(products, structures, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search products for term length {Len}.", q.Length);
            return [];
        }
    }

    public async Task<byte[]?> GetCategoryIconAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"store:category-icon:{categoryId}";
        if (_cache.TryGetValue(cacheKey, out byte[]? cached) && cached is { Length: > 0 })
        {
            return cached;
        }

        try
        {
            return await RunSerializedAsync(async () =>
            {
                if (_cache.TryGetValue(cacheKey, out cached) && cached is { Length: > 0 })
                {
                    return cached;
                }

                var icon = await _db.ProductStructures.AsNoTracking()
                    .Where(s => s.Id == categoryId)
                    .Select(s => s.Icon)
                    .FirstOrDefaultAsync(cancellationToken);

                if (icon is { Length: > 0 })
                {
                    _cache.Set(cacheKey, icon, TimeSpan.FromMinutes(30));
                    return icon;
                }

                return null;
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Browser aborted the img request (navigate away / new page) — not a fault.
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
                    x.OrderPartNumber,
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
            var hasOptions = (await SafeGetProductIdsWithOptionsAsync([productId], cancellationToken)).Count > 0;

            return MapProduct(
                p.ProductId,
                p.IsNew,
                p.NameEn,
                p.WebshopDescriptionNl,
                p.OrderPartNumber,
                await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: true, cancellationToken)
                    ?? FallbackImage(productId),
                level.Quantity,
                level.MinQuantity,
                ResolveStorePrice(p.HasNoPrice, price),
                categoryIdValue,
                categoryRootId,
                categoryName,
                hasOptions);
        }
        catch
        {
            return null;
        }
    }

    public Task<IReadOnlyList<StoreProductOptionDto>> GetProductOptionsAsync(int productId, CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetProductOptionsCoreAsync(productId, cancellationToken), cancellationToken);

    private async Task<IReadOnlyList<StoreProductOptionDto>> GetProductOptionsCoreAsync(int productId, CancellationToken cancellationToken)
    {
        try
        {
            var options = await _db.ProductOptions.AsNoTracking()
                .Where(o => o.ProductId == productId)
                .OrderBy(o => o.SortOrder)
                .ThenBy(o => o.Id)
                .Select(o => new { o.Id, o.Name, o.NameEn, o.ValueType, o.IsRequired, o.SortOrder })
                .ToListAsync(cancellationToken);

            if (options.Count == 0)
            {
                return [];
            }

            var optionIds = options.Select(o => o.Id).ToList();
            var values = await _db.ProductOptionValues.AsNoTracking()
                .Where(v => optionIds.Contains(v.ProductOptionId))
                .OrderBy(v => v.SortOrder)
                .ThenBy(v => v.Id)
                .Select(v => new { v.Id, v.Value, v.ValueEn, v.ProductOptionId, v.SortOrder })
                .ToListAsync(cancellationToken);

            var valuesByOption = values
                .GroupBy(v => v.ProductOptionId)
                .ToDictionary(
                    g => g.Key,
                    g => (IReadOnlyList<StoreProductOptionValueDto>)g
                        .Select(v => new StoreProductOptionValueDto
                        {
                            Id = v.Id,
                            Value = PickText(v.ValueEn, v.Value),
                            SortOrder = v.SortOrder
                        })
                        .Where(v => !string.IsNullOrWhiteSpace(v.Value))
                        .ToList());

            return options
                .Select(o => new StoreProductOptionDto
                {
                    Id = o.Id,
                    Name = PickText(o.NameEn, o.Name),
                    ValueType = o.ValueType ?? string.Empty,
                    IsRequired = o.IsRequired,
                    SortOrder = o.SortOrder,
                    Values = valuesByOption.GetValueOrDefault(o.Id, [])
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load product options for product {ProductId}.", productId);
            return [];
        }
    }

    public Task<StoreCategoryDetailDto?> GetCategoryDetailAsync(int categoryId, CancellationToken cancellationToken = default) =>
        RunSerializedAsync(() => GetCategoryDetailCoreAsync(categoryId, cancellationToken), cancellationToken);

    private async Task<StoreCategoryDetailDto?> GetCategoryDetailCoreAsync(int categoryId, CancellationToken cancellationToken)
    {
        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);
            if (!structures.TryGetValue(categoryId, out var structure))
            {
                return null;
            }

            var textIds = new List<int>();
            if (structure.IntroPriceListTextId is int introId) textIds.Add(introId);
            if (structure.OutroPriceListTextId is int outroId) textIds.Add(outroId);

            Dictionary<int, (string? En, string? Nl)> texts = new();
            if (textIds.Count > 0)
            {
                texts = (await _db.PriceListTexts.AsNoTracking()
                        .Where(t => textIds.Contains(t.Id))
                        .Select(t => new { t.Id, t.TextEn, t.Text })
                        .ToListAsync(cancellationToken))
                    .ToDictionary(t => t.Id, t => ((string?)t.TextEn, (string?)t.Text));
            }

            return new StoreCategoryDetailDto
            {
                Id = structure.Id,
                Name = CatalogCategoryTree.PickStorefrontName(structure),
                IntroText = ResolveText(structure.IntroPriceListTextId, texts),
                OutroText = ResolveText(structure.OutroPriceListTextId, texts)
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load category detail for {CategoryId}.", categoryId);
            return null;
        }
    }

    private static string? ResolveText(int? textId, IReadOnlyDictionary<int, (string? En, string? Nl)> texts)
    {
        if (textId is not int id || !texts.TryGetValue(id, out var value))
        {
            return null;
        }

        var resolved = PickText(value.En, value.Nl);
        if (string.IsNullOrWhiteSpace(resolved) || IsRichTextMarkup(resolved))
        {
            // Price-list intros are often stored as RTF/binary in the ERP — never dump raw markup on the storefront.
            return null;
        }

        return resolved;
    }

    private static bool IsRichTextMarkup(string text)
    {
        var trimmed = text.TrimStart();
        return trimmed.StartsWith(@"{\rtf", StringComparison.OrdinalIgnoreCase)
               || trimmed.StartsWith("{\\rtf", StringComparison.OrdinalIgnoreCase);
    }

    private static string PickText(string? english, string? dutch) =>
        !string.IsNullOrWhiteSpace(english) ? english.Trim()
        : !string.IsNullOrWhiteSpace(dutch) ? dutch.Trim()
        : string.Empty;

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

    private static List<StoreCategoryTreeNodeDto> BuildProductStructureNavTree(
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<int, int> counts,
        IReadOnlyDictionary<int, bool> hasIcon,
        int? parentId)
    {
        var candidates = parentId is null
            ? structures.Values.Where(s => CatalogCategoryTree.IsStructuralRoot(s, structures)
                                           && s.ShowOnPriceList == true)
            : structures.Values.Where(s => CatalogCategoryTree.NormalizeParentId(s.ParentTaskId) == parentId
                                           && s.ShowOnPriceList == true);

        return candidates
            .OrderBy(s => s.SortOrder)
            .ThenBy(s => s.Id)
            .Select(s => new StoreCategoryTreeNodeDto
            {
                Id = s.Id,
                NavigationKey = s.Id,
                ParentId = CatalogCategoryTree.NormalizeParentId(s.ParentTaskId),
                Name = CatalogCategoryTree.PickStorefrontName(s),
                Level = s.Level,
                ProductCount = counts.GetValueOrDefault(s.Id),
                HasIcon = hasIcon.GetValueOrDefault(s.Id),
                Children = BuildProductStructureNavTree(structures, counts, hasIcon, s.Id)
            })
            .ToList();
    }

    private async Task<List<StoreCategoryTreeNodeDto>> LoadWebshopNavTreeAsync(
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<int, int> counts,
        IReadOnlyDictionary<int, bool> hasIcon,
        CancellationToken cancellationToken)
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
        var nameIndex = BuildProductStructureNameIndex(structures);
        return rows
            .Where(s => IsWebshopStructuralRoot(s, byId))
            .Select(s => MapWebshopNavNode(s, rows, byId, structures, counts, hasIcon, nameIndex))
            .ToList();
    }

    private static StoreCategoryTreeNodeDto MapWebshopNavNode(
        WebshopStructure node,
        IReadOnlyList<WebshopStructure> rows,
        IReadOnlyDictionary<int, WebshopStructure> webshopById,
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<int, int> counts,
        IReadOnlyDictionary<int, bool> hasIcon,
        IReadOnlyDictionary<string, List<int>> nameIndex)
    {
        var catalogId = ResolveCatalogCategoryId(node, rows, webshopById, structures, nameIndex, counts);
        structures.TryGetValue(catalogId, out var structure);
        var children = BuildWebshopNavChildren(rows, node.Id, webshopById, structures, counts, hasIcon, nameIndex);

        return new StoreCategoryTreeNodeDto
        {
            Id = catalogId,
            NavigationKey = node.Id,
            ParentId = CatalogCategoryTree.NormalizeParentId(node.ParentTaskId),
            Name = node.NameNl.Trim(),
            Level = structure?.Level ?? 0,
            ProductCount = counts.GetValueOrDefault(catalogId),
            HasIcon = hasIcon.GetValueOrDefault(catalogId),
            Children = children
        };
    }

    private static List<StoreCategoryTreeNodeDto> BuildWebshopNavChildren(
        IReadOnlyList<WebshopStructure> rows,
        int parentId,
        IReadOnlyDictionary<int, WebshopStructure> webshopById,
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<int, int> counts,
        IReadOnlyDictionary<int, bool> hasIcon,
        IReadOnlyDictionary<string, List<int>> nameIndex) =>
        rows
            .Where(s => CatalogCategoryTree.NormalizeParentId(s.ParentTaskId) == parentId)
            .OrderBy(s => s.SortOrder)
            .ThenBy(s => s.NameNl, StringComparer.OrdinalIgnoreCase)
            .Select(s => MapWebshopNavNode(s, rows, webshopById, structures, counts, hasIcon, nameIndex))
            .ToList();

    private static List<StoreCategoryTreeNodeDto> PruneEmptyCategories(
        IReadOnlyList<StoreCategoryTreeNodeDto> nodes) =>
        nodes
            .Select(PruneEmptyCategoryNode)
            .Where(n => n is not null)
            .Select(n => n!)
            .ToList();

    private static StoreCategoryTreeNodeDto? PruneEmptyCategoryNode(StoreCategoryTreeNodeDto node)
    {
        var children = PruneEmptyCategories(node.Children);
        var count = node.ProductCount ?? 0;
        if (count <= 0 && children.Count == 0)
        {
            return null;
        }

        return new StoreCategoryTreeNodeDto
        {
            Id = node.Id,
            NavigationKey = node.NavigationKey,
            ParentId = node.ParentId,
            Name = node.Name,
            Level = node.Level,
            ProductCount = node.ProductCount,
            HasIcon = node.HasIcon,
            Children = children
        };
    }

    private static int ResolveCatalogCategoryId(
        WebshopStructure node,
        IReadOnlyList<WebshopStructure> rows,
        IReadOnlyDictionary<int, WebshopStructure> webshopById,
        IReadOnlyDictionary<int, ProductStructure> structures,
        IReadOnlyDictionary<string, List<int>> nameIndex,
        IReadOnlyDictionary<int, int> counts)
    {
        if (structures.ContainsKey(node.Id))
        {
            return node.Id;
        }

        var webshopPath = BuildWebshopPath(node, webshopById);
        var normalized = NormalizeCategoryName(node.NameNl);
        if (!nameIndex.TryGetValue(normalized, out var candidates) || candidates.Count == 0)
        {
            return node.Id;
        }

        if (candidates.Count == 1)
        {
            return candidates[0];
        }

        return candidates
            .Select(id => (id, score: ScorePathMatch(webshopPath, BuildProductStructurePath(id, structures))))
            .OrderByDescending(x => x.score)
            .ThenByDescending(x => CatalogCategoryTree.HasStructuralChildren(x.id, structures) ? 0 : 1)
            .ThenByDescending(x => counts.GetValueOrDefault(x.id))
            .ThenByDescending(x => structures.TryGetValue(x.id, out var s) ? s.Level : 0)
            .First().id;
    }

    private static List<string> BuildWebshopPath(
        WebshopStructure node,
        IReadOnlyDictionary<int, WebshopStructure> webshopById)
    {
        var path = new List<string> { NormalizeCategoryName(node.NameNl) };
        var parentId = CatalogCategoryTree.NormalizeParentId(node.ParentTaskId);
        var guard = 0;
        while (parentId is int pid && webshopById.TryGetValue(pid, out var parent) && guard++ < 12)
        {
            path.Insert(0, NormalizeCategoryName(parent.NameNl));
            parentId = CatalogCategoryTree.NormalizeParentId(parent.ParentTaskId);
        }

        return path;
    }

    private static List<string> BuildProductStructurePath(
        int structureId,
        IReadOnlyDictionary<int, ProductStructure> structures)
    {
        var path = new List<string>();
        var currentId = structureId;
        var guard = 0;
        while (structures.TryGetValue(currentId, out var current) && guard++ < 12)
        {
            path.Insert(0, NormalizeCategoryName(CatalogCategoryTree.PickStorefrontName(current)));
            var parentId = CatalogCategoryTree.NormalizeParentId(current.ParentTaskId);
            if (parentId is null)
            {
                break;
            }

            currentId = parentId.Value;
        }

        return path;
    }

    private static int ScorePathMatch(IReadOnlyList<string> webshopPath, IReadOnlyList<string> productPath)
    {
        var score = 0;
        foreach (var name in webshopPath)
        {
            if (productPath.Any(p => string.Equals(p, name, StringComparison.OrdinalIgnoreCase)))
            {
                score++;
            }
        }

        if (webshopPath.Count > 0
            && productPath.Count > 0
            && string.Equals(webshopPath[^1], productPath[^1], StringComparison.OrdinalIgnoreCase))
        {
            score += 2;
        }

        return score;
    }

    private static Dictionary<string, List<int>> BuildProductStructureNameIndex(
        IReadOnlyDictionary<int, ProductStructure> structures)
    {
        var index = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
        foreach (var structure in structures.Values)
        {
            foreach (var candidate in new[] { structure.NameNl, structure.NameEn, structure.NameFr })
            {
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    continue;
                }

                var key = NormalizeCategoryName(candidate);
                if (!index.TryGetValue(key, out var ids))
                {
                    ids = [];
                    index[key] = ids;
                }

                if (!ids.Contains(structure.Id))
                {
                    ids.Add(structure.Id);
                }
            }
        }

        return index;
    }

    private static string NormalizeCategoryName(string value) =>
        string.Join(' ', value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));

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

    private IQueryable<Product> QueryVisibleProducts() =>
        _db.Products.AsNoTracking()
            .Where(p => (p.ShowOnWebshop ?? false) && !p.IsInactive);

    /// <summary>Webshop products linked to a real category — skips orphan / legacy junk rows.</summary>
    private IQueryable<Product> QueryVisibleLinkedProducts() =>
        QueryVisibleProducts().Where(p => p.ProductStructureId != null && p.ProductStructureId > 0);

    private async Task<Dictionary<int, ProductStructure>> LoadProductStructuresAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(ProductStructuresCacheKey, out Dictionary<int, ProductStructure>? cached)
            && cached is not null)
        {
            return cached;
        }

        // Never select Icon bytes — only a flag. Blobs made cold homepage loads 40s+.
        var rows = await _db.ProductStructures.AsNoTracking()
            .Select(s => new
            {
                s.Id,
                s.Level,
                s.ParentTaskId,
                s.NameNl,
                s.NameEn,
                s.NameFr,
                s.IntroPriceListTextId,
                s.OutroPriceListTextId,
                s.SortOrder,
                s.Color,
                s.ShowOnPriceList,
                s.PageBreakAfter,
                HasIcon = s.Icon != null
            })
            .ToListAsync(cancellationToken);

        var structures = rows.ToDictionary(
            s => s.Id,
            s => new ProductStructure
            {
                Id = s.Id,
                Level = s.Level,
                ParentTaskId = s.ParentTaskId,
                NameNl = s.NameNl,
                NameEn = s.NameEn,
                NameFr = s.NameFr,
                IntroPriceListTextId = s.IntroPriceListTextId,
                OutroPriceListTextId = s.OutroPriceListTextId,
                SortOrder = s.SortOrder,
                Color = s.Color,
                ShowOnPriceList = s.ShowOnPriceList,
                PageBreakAfter = s.PageBreakAfter,
                Icon = null
            });

        var hasIcon = rows.Where(s => s.HasIcon).ToDictionary(s => s.Id, _ => true);
        _cache.Set(ProductStructuresCacheKey, structures, TimeSpan.FromMinutes(10));
        _cache.Set(StructureHasIconCacheKey, hasIcon, TimeSpan.FromMinutes(10));
        return structures;
    }

    private async Task<IReadOnlyDictionary<int, bool>> LoadStructureHasIconFlagsAsync(
        CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(StructureHasIconCacheKey, out Dictionary<int, bool>? cached)
            && cached is not null)
        {
            return cached;
        }

        // Populate via structures load (stores both caches).
        await LoadProductStructuresAsync(cancellationToken);
        return _cache.TryGetValue(StructureHasIconCacheKey, out cached) && cached is not null
            ? cached
            : new Dictionary<int, bool>();
    }

    private static async Task<List<CatalogProductRow>> LoadProductRowsAsync(
        IQueryable<Product> query,
        int? take,
        CancellationToken cancellationToken)
    {
        var ordered = query
            .OrderBy(p => p.PriceListSortOrder ?? int.MaxValue)
            .ThenBy(p => p.NameEn);

        var limited = take is > 0 ? ordered.Take(take.Value) : ordered;

        var rows = await limited
            .Select(p => new
            {
                p.ProductId,
                IsNew = p.IsNew == true,
                p.NameEn,
                p.DescriptionEn,
                p.WebshopDescriptionNl,
                p.OrderPartNumber,
                p.PriceListSortOrder,
                p.ProductStructureId,
                p.HasNoPrice,
                p.ManufacturerId
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(p => new CatalogProductRow(
                p.ProductId,
                p.IsNew,
                p.NameEn ?? string.Empty,
                p.WebshopDescriptionNl ?? p.DescriptionEn ?? string.Empty,
                p.OrderPartNumber ?? string.Empty,
                p.PriceListSortOrder,
                p.ProductStructureId,
                p.HasNoPrice,
                p.ManufacturerId))
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
        var withOptions = await SafeGetProductIdsWithOptionsAsync(productIds, cancellationToken);
        var manufacturerNames = await SafeGetManufacturerNamesAsync(
            products.Select(p => p.ManufacturerId).Where(id => id > 0).Distinct().ToList(),
            cancellationToken);

        var items = new List<StoreProductDto>();
        foreach (var p in products)
        {
            stockLevels.TryGetValue(p.ProductId, out var level);
            var (categoryIdValue, categoryRootId, categoryName) = ResolveCategory(p.ProductStructureId, structures);
            var manufacturerName = p.ManufacturerId > 0
                ? manufacturerNames.GetValueOrDefault(p.ManufacturerId) ?? string.Empty
                : string.Empty;

            items.Add(MapProduct(
                p.ProductId,
                p.IsNew,
                p.NameEn,
                p.WebshopDescriptionNl,
                p.OrderPartNumber,
                imageUrls.GetValueOrDefault(p.ProductId) ?? FallbackImage(p.ProductId),
                level.Quantity,
                level.MinQuantity,
                ResolveStorePrice(p.HasNoPrice, prices, p.ProductId),
                categoryIdValue,
                categoryRootId,
                categoryName,
                withOptions.Contains(p.ProductId),
                p.ManufacturerId,
                manufacturerName));
        }

        return items;
    }

    private async Task<IReadOnlyDictionary<int, string>> SafeGetManufacturerNamesAsync(
        IReadOnlyList<int> manufacturerIds,
        CancellationToken cancellationToken)
    {
        if (manufacturerIds.Count == 0)
        {
            return new Dictionary<int, string>();
        }

        try
        {
            return await _db.Manufacturers.AsNoTracking()
                .Where(m => manufacturerIds.Contains(m.ManufacturerId))
                .Select(m => new { m.ManufacturerId, m.Name })
                .ToDictionaryAsync(m => m.ManufacturerId, m => m.Name ?? string.Empty, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load manufacturers for {Count} ids.", manufacturerIds.Count);
            return new Dictionary<int, string>();
        }
    }

    private async Task<IReadOnlyDictionary<int, IReadOnlyList<(int PropertyId, string PropertyName, string Value)>>> LoadPropertyValuesAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<int, IReadOnlyList<(int, string, string)>>();
        }

        try
        {
            var rows = await (
                from item in _db.ProductPropertyItems.AsNoTracking()
                join prop in _db.ProductProperties.AsNoTracking() on item.ProductPropertyId equals prop.Id
                where productIds.Contains(item.ProductId)
                select new
                {
                    item.ProductId,
                    item.ProductPropertyId,
                    PropertyName = prop.NameNl,
                    prop.SortOrder,
                    item.Value
                }).ToListAsync(cancellationToken);

            return rows
                .GroupBy(r => r.ProductId)
                .ToDictionary(
                    g => g.Key,
                    IReadOnlyList<(int, string, string)> (g) => g
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.PropertyName)
                        .Select(x => (x.ProductPropertyId, x.PropertyName ?? string.Empty, x.Value ?? string.Empty))
                        .ToList());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load ProductProperty items for {Count} products.", productIds.Count);
            return new Dictionary<int, IReadOnlyList<(int, string, string)>>();
        }
    }

    private static IReadOnlyList<StoreProductDto> ApplyFilters(
        IReadOnlyList<StoreProductDto> products,
        StoreCatalogFilterState filters,
        IReadOnlyDictionary<int, IReadOnlyList<(int PropertyId, string PropertyName, string Value)>> propertyMap)
    {
        if (!filters.HasAny)
        {
            return products;
        }

        IEnumerable<StoreProductDto> query = products;

        if (filters.ManufacturerIds.Count > 0 || filters.IncludeUnknownManufacturer)
        {
            var ids = filters.ManufacturerIds.ToHashSet();
            query = query.Where(p =>
                (p.ManufacturerId > 0 && ids.Contains(p.ManufacturerId))
                || (filters.IncludeUnknownManufacturer && p.ManufacturerId <= 0));
        }

        if (filters.StockKeys.Count > 0)
        {
            var stock = filters.StockKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);
            query = query.Where(p =>
                (stock.Contains("in") && !p.IsOutOfStock)
                || (stock.Contains("out") && p.IsOutOfStock));
        }

        if (filters.PriceKeys.Count > 0)
        {
            var price = filters.PriceKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);
            query = query.Where(p =>
                (price.Contains("request") && !p.HasPrice)
                || (price.Contains("priced") && p.HasPrice));
        }

        foreach (var (propertyId, values) in filters.PropertyValues)
        {
            if (values.Count == 0)
            {
                continue;
            }

            var wanted = values.ToHashSet(StringComparer.OrdinalIgnoreCase);
            query = query.Where(p =>
                propertyMap.TryGetValue(p.Id, out var props)
                && props.Any(x => x.PropertyId == propertyId && wanted.Contains(x.Value)));
        }

        return query.ToList();
    }

    private IReadOnlyList<StoreFacetGroupDto> BuildFacetGroups(
        IReadOnlyList<StoreProductDto> all,
        IReadOnlyList<StoreProductDto> matched,
        StoreCatalogFilterState active,
        IReadOnlyDictionary<int, IReadOnlyList<(int PropertyId, string PropertyName, string Value)>> propertyMap,
        int categoryId)
    {
        var groups = new List<StoreFacetGroupDto>();

        // Counts for each group use products matching all OTHER filters (Coolblue-style).
        var forBrand = ApplyFilters(all, WithoutManufacturer(active), propertyMap);
        var brandValues = forBrand
            .GroupBy(p => p.ManufacturerId)
            .Select(g =>
            {
                var id = g.Key;
                var label = id > 0
                    ? (g.Select(x => x.ManufacturerName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? $"#{id}")
                    : "Geen";
                var value = id > 0 ? id.ToString() : "none";
                var selected = id > 0
                    ? active.ManufacturerIds.Contains(id)
                    : active.IncludeUnknownManufacturer;
                return new StoreFacetValueDto
                {
                    Value = value,
                    Label = label,
                    Count = g.Count(),
                    Selected = selected
                };
            })
            .OrderByDescending(v => v.Value == "none" ? 0 : 1)
            .ThenBy(v => v.Label, StringComparer.OrdinalIgnoreCase)
            .ToList();

        groups.Add(new StoreFacetGroupDto
        {
            Key = "manufacturer",
            Title = "Merk",
            Values = brandValues
        });

        var forStock = ApplyFilters(all, WithoutStock(active), propertyMap);
        groups.Add(new StoreFacetGroupDto
        {
            Key = "stock",
            Title = "Voorraad",
            Values =
            [
                new StoreFacetValueDto
                {
                    Value = "in",
                    Label = "Op voorraad",
                    Count = forStock.Count(p => !p.IsOutOfStock),
                    Selected = active.StockKeys.Contains("in", StringComparer.OrdinalIgnoreCase)
                },
                new StoreFacetValueDto
                {
                    Value = "out",
                    Label = "Uit voorraad",
                    Count = forStock.Count(p => p.IsOutOfStock),
                    Selected = active.StockKeys.Contains("out", StringComparer.OrdinalIgnoreCase)
                }
            ]
        });

        var forPrice = ApplyFilters(all, WithoutPrice(active), propertyMap);
        groups.Add(new StoreFacetGroupDto
        {
            Key = "price",
            Title = "Prijs",
            Values =
            [
                new StoreFacetValueDto
                {
                    Value = "priced",
                    Label = "Met prijs",
                    Count = forPrice.Count(p => p.HasPrice),
                    Selected = active.PriceKeys.Contains("priced", StringComparer.OrdinalIgnoreCase)
                },
                new StoreFacetValueDto
                {
                    Value = "request",
                    Label = "Prijs op aanvraag",
                    Count = forPrice.Count(p => !p.HasPrice),
                    Selected = active.PriceKeys.Contains("request", StringComparer.OrdinalIgnoreCase)
                }
            ]
        });

        var propertyGroups = BuildPropertyFacetGroups(all, active, propertyMap, categoryId);
        if (propertyGroups.Count > 0)
        {
            groups.AddRange(propertyGroups);
        }
        else
        {
            groups.Add(new StoreFacetGroupDto
            {
                Key = "property-placeholder",
                Title = "Processor, RAM, opslag…",
                IsMuted = true,
                Note = "Geen data in ProductProperty vandaag — toekomstige fase (Coolblue-stijl).",
                Values = []
            });
        }

        return groups;
    }

    private IReadOnlyList<StoreFacetGroupDto> BuildPropertyFacetGroups(
        IReadOnlyList<StoreProductDto> all,
        StoreCatalogFilterState active,
        IReadOnlyDictionary<int, IReadOnlyList<(int PropertyId, string PropertyName, string Value)>> propertyMap,
        int categoryId)
    {
        var flat = all
            .SelectMany(p => propertyMap.TryGetValue(p.Id, out var props)
                ? props.Select(x => (p.Id, x.PropertyId, x.PropertyName, x.Value))
                : [])
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .ToList();

        if (flat.Count == 0)
        {
            return [];
        }

        var order = _filterOptions.GetPropertyOrder(categoryId);
        var byProperty = flat.GroupBy(x => x.PropertyId).ToList();
        if (order is { Count: > 0 })
        {
            byProperty = byProperty
                .OrderBy(g =>
                {
                    var idx = order.ToList().IndexOf(g.Key);
                    return idx < 0 ? int.MaxValue : idx;
                })
                .ThenBy(g => g.First().PropertyName, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        else
        {
            byProperty = byProperty
                .OrderBy(g => g.First().PropertyName, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        var result = new List<StoreFacetGroupDto>();
        foreach (var group in byProperty)
        {
            var propertyId = group.Key;
            var title = group.First().PropertyName;
            var selectedValues = active.PropertyValues.TryGetValue(propertyId, out var sel)
                ? sel.ToHashSet(StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var withoutThis = WithoutProperty(active, propertyId);
            var pool = ApplyFilters(all, withoutThis, propertyMap);
            var poolIds = pool.Select(p => p.Id).ToHashSet();

            var values = group
                .Where(x => poolIds.Contains(x.Id))
                .GroupBy(x => x.Value, StringComparer.OrdinalIgnoreCase)
                .Select(g => new StoreFacetValueDto
                {
                    Value = g.Key,
                    Label = g.Key,
                    Count = g.Select(x => x.Id).Distinct().Count(),
                    Selected = selectedValues.Contains(g.Key)
                })
                .OrderBy(v => v.Label, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (values.Count == 0)
            {
                continue;
            }

            result.Add(new StoreFacetGroupDto
            {
                Key = $"property:{propertyId}",
                Title = string.IsNullOrWhiteSpace(title) ? $"Property {propertyId}" : title,
                Values = values
            });
        }

        return result;
    }

    private static StoreCatalogFilterState WithoutManufacturer(StoreCatalogFilterState s) =>
        new()
        {
            StockKeys = s.StockKeys,
            PriceKeys = s.PriceKeys,
            PropertyValues = s.PropertyValues
        };

    private static StoreCatalogFilterState WithoutStock(StoreCatalogFilterState s) =>
        new()
        {
            ManufacturerIds = s.ManufacturerIds,
            IncludeUnknownManufacturer = s.IncludeUnknownManufacturer,
            PriceKeys = s.PriceKeys,
            PropertyValues = s.PropertyValues
        };

    private static StoreCatalogFilterState WithoutPrice(StoreCatalogFilterState s) =>
        new()
        {
            ManufacturerIds = s.ManufacturerIds,
            IncludeUnknownManufacturer = s.IncludeUnknownManufacturer,
            StockKeys = s.StockKeys,
            PropertyValues = s.PropertyValues
        };

    private static StoreCatalogFilterState WithoutProperty(StoreCatalogFilterState s, int propertyId)
    {
        var copy = s.PropertyValues
            .Where(kv => kv.Key != propertyId)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        return new StoreCatalogFilterState
        {
            ManufacturerIds = s.ManufacturerIds,
            IncludeUnknownManufacturer = s.IncludeUnknownManufacturer,
            StockKeys = s.StockKeys,
            PriceKeys = s.PriceKeys,
            PropertyValues = copy
        };
    }

    private async Task<HashSet<int>> SafeGetProductIdsWithOptionsAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return [];
        }

        try
        {
            var ids = await _db.ProductOptions.AsNoTracking()
                .Where(o => productIds.Contains(o.ProductId))
                .Select(o => o.ProductId)
                .Distinct()
                .ToListAsync(cancellationToken);
            return [.. ids];
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load product options flags for {Count} products.", productIds.Count);
            return [];
        }
    }

    private static (int? CategoryId, int? CategoryRootId, string CategoryName) ResolveCategory(        int? structureId,
        IReadOnlyDictionary<int, ProductStructure> structures)
    {
        if (structureId is not int id || !structures.TryGetValue(id, out var structure))
        {
            return (null, null, string.Empty);
        }

        var rootId = CatalogCategoryTree.ResolveRootId(id, structures);
        return (id, rootId, CatalogCategoryTree.PickStorefrontName(structure));
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
        string OrderPartNumber,
        int? PriceListSortOrder,
        int? ProductStructureId,
        bool HasNoPrice,
        int ManufacturerId);

    private static StoreProductDto MapProduct(
        int id,
        bool isNew,
        string name,
        string description,
        string referenceCode,
        string imageUrl,
        int qty,
        decimal minQuantity,
        decimal? price,
        int? categoryId,
        int? categoryRootId,
        string categoryName,
        bool hasOptions = false,
        int manufacturerId = 0,
        string manufacturerName = "") =>
        new()
        {
            Id = id,
            IsNew = isNew,
            Name = name,
            Description = description,
            ReferenceCode = referenceCode.Trim(),
            ImageUrl = imageUrl,
            Price = price,
            Stock = qty,
            MinQuantity = minQuantity,
            CategoryId = categoryId,
            CategoryRootId = categoryRootId,
            CategoryName = categoryName,
            HasOptions = hasOptions,
            ManufacturerId = manufacturerId,
            ManufacturerName = manufacturerName
        };
}
