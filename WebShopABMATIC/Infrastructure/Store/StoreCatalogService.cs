using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Store;

public sealed class StoreCatalogService : IStoreCatalogPort
{
    private readonly WebShopABMATICDbContext _db;
    private readonly IProductMediaPort _media;
    private readonly IProductPricingPort _pricing;

    public StoreCatalogService(WebShopABMATICDbContext db, IProductMediaPort media, IProductPricingPort pricing)
    {
        _db = db;
        _media = media;
        _pricing = pricing;
    }

    public async Task<IReadOnlyList<StoreCatalogCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var webshopNav = await _db.WebshopStructures.AsNoTracking()
                .OrderBy(s => s.SortOrder)
                .Select(s => new StoreCatalogCategoryDto
                {
                    Id = s.Id,
                    Name = s.NameNl,
                    ProductCount = 0
                })
                .ToListAsync(cancellationToken);

            if (webshopNav.Count > 0)
            {
                return webshopNav;
            }

            var structures = await LoadProductStructuresAsync(cancellationToken);
            var productStructureIds = await _db.Products.AsNoTracking()
                .Where(p => p.ShowOnWebshop == true && !p.IsInactive && p.ProductStructureId != null)
                .Select(p => p.ProductStructureId!.Value)
                .ToListAsync(cancellationToken);

            var rootCounts = new Dictionary<int, int>();
            foreach (var structureId in productStructureIds)
            {
                var rootId = CatalogCategoryTree.ResolveRootId(structureId, structures);
                rootCounts[rootId] = rootCounts.GetValueOrDefault(rootId) + 1;
            }

            return rootCounts
                .Where(kv => structures.ContainsKey(kv.Key))
                .Select(kv => new StoreCatalogCategoryDto
                {
                    Id = kv.Key,
                    Name = CatalogCategoryTree.PickDisplayName(structures[kv.Key]),
                    ProductCount = kv.Value
                })
                .OrderBy(c => c.Name)
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    public async Task<IReadOnlyList<StoreProductDto>> GetCatalogAsync(
        int? take = null,
        int? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var structures = await LoadProductStructuresAsync(cancellationToken);
            HashSet<int>? allowedStructureIds = null;
            if (categoryId is > 0)
            {
                var webshopNavExists = await _db.WebshopStructures.AsNoTracking()
                    .AnyAsync(s => s.Id == categoryId, cancellationToken);
                if (webshopNavExists)
                {
                    allowedStructureIds = null;
                }
                else if (structures.ContainsKey(categoryId.Value))
                {
                    allowedStructureIds = CatalogCategoryTree.CollectDescendantIds(categoryId.Value, structures);
                }
            }

            var query = _db.Products.AsNoTracking()
                .Where(p => p.ShowOnWebshop == true && !p.IsInactive);

            if (allowedStructureIds is not null)
            {
                query = query.Where(p => p.ProductStructureId != null && allowedStructureIds.Contains(p.ProductStructureId.Value));
            }

            var ordered = query.OrderBy(p => p.NameEn)
                .Select(p => new
                {
                    p.ProductId,
                    p.NameEn,
                    p.WebshopDescriptionNl,
                    p.ProductStructureId,
                    p.HasNoPrice
                });

            var products = take is > 0
                ? await ordered.Take(take.Value).ToListAsync(cancellationToken)
                : await ordered.ToListAsync(cancellationToken);

            var productIds = products.Select(p => p.ProductId).ToList();
            var prices = await _pricing.GetCatalogPricesAsync(productIds, cancellationToken: cancellationToken);
            var stockLevels = await GetDefaultStockLevelsAsync(productIds, cancellationToken);

            var items = new List<StoreProductDto>();
            foreach (var p in products)
            {
                prices.TryGetValue(p.ProductId, out var price);
                stockLevels.TryGetValue(p.ProductId, out var level);
                var (categoryIdValue, categoryRootId, categoryName) = ResolveCategory(p.ProductStructureId, structures);

                items.Add(MapProduct(
                    p.ProductId,
                    p.NameEn,
                    p.WebshopDescriptionNl,
                    FallbackImage(p.ProductId),
                    level.Quantity,
                    level.MinQuantity,
                    p.HasNoPrice ? null : price,
                    categoryIdValue,
                    categoryRootId,
                    categoryName));
            }

            return items;
        }
        catch
        {
            return [];
        }
    }

    public async Task<StoreProductDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            var p = await _db.Products.AsNoTracking()
                .Where(x => x.ProductId == productId && x.ShowOnWebshop == true && !x.IsInactive)
                .Select(x => new
                {
                    x.ProductId,
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
                p.NameEn,
                p.WebshopDescriptionNl,
                await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: true, cancellationToken)
                    ?? FallbackImage(productId),
                level.Quantity,
                level.MinQuantity,
                p.HasNoPrice ? null : price,
                categoryIdValue,
                categoryRootId,
                categoryName);
        }
        catch
        {
            return null;
        }
    }

    private async Task<Dictionary<int, ProductStructure>> LoadProductStructuresAsync(CancellationToken cancellationToken)
    {
        var rows = await _db.ProductStructures.AsNoTracking().ToListAsync(cancellationToken);
        return rows.ToDictionary(s => s.Id);
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

    private static StoreProductDto MapProduct(
        int id,
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

    private static string FallbackImage(int productId) =>
        productId is >= 1 and <= 6 ? $"/images/product{productId}.png" : "/images/product1.png";
}
