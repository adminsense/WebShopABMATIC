using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store;
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

    public async Task<IReadOnlyList<StoreProductDto>> GetCatalogAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await _db.Products.AsNoTracking()
                .Where(p => p.ShowOnWebshop == true && !p.IsInactive)
                .OrderBy(p => p.NameEn)
                .Select(p => new
                {
                    p.ProductId,
                    p.NameEn,
                    p.WebshopDescriptionNl,
                    p.OrderPartNumber
                })
                .ToListAsync(cancellationToken);

            var productIds = products.Select(p => p.ProductId).ToList();
            var prices = await _pricing.GetCatalogPricesAsync(productIds, cancellationToken: cancellationToken);
            var stockLevels = await GetDefaultStockLevelsAsync(productIds, cancellationToken);

            var items = new List<StoreProductDto>();
            foreach (var p in products)
            {
                var imageUrl = await _media.GetPrimaryImageUrlAsync(p.ProductId, webPublishedOnly: true, cancellationToken)
                    ?? FallbackImage(p.ProductId);
                prices.TryGetValue(p.ProductId, out var price);
                stockLevels.TryGetValue(p.ProductId, out var level);
                items.Add(MapProduct(
                    p.ProductId,
                    p.NameEn,
                    p.WebshopDescriptionNl,
                    p.OrderPartNumber,
                    imageUrl,
                    level.Quantity,
                    level.MinQuantity,
                    price));
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
                    x.OrderPartNumber
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (p is null)
            {
                return null;
            }

            var stockLevels = await GetDefaultStockLevelsAsync([productId], cancellationToken);
            stockLevels.TryGetValue(productId, out var level);

            var price = await _pricing.GetUnitPriceAsync(productId, cancellationToken: cancellationToken);

            var imageUrl = await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: true, cancellationToken)
                ?? FallbackImage(productId);

            return MapProduct(
                p.ProductId,
                p.NameEn,
                p.WebshopDescriptionNl,
                p.OrderPartNumber,
                imageUrl,
                level.Quantity,
                level.MinQuantity,
                price);
        }
        catch
        {
            return null;
        }
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
        string partNumber,
        string imageUrl,
        int qty,
        decimal minQuantity,
        decimal? price)
    {
        var category = InferCategory(partNumber);
        return new StoreProductDto
        {
            Id = id,
            Name = name,
            Description = description,
            ImageUrl = imageUrl,
            Price = price ?? 0m,
            Stock = qty,
            MinQuantity = minQuantity,
            Category = category,
            Tag = category == "ssd" ? "SSD" : category == "hdd" ? "HDD" : "Storage"
        };
    }

    private static string InferCategory(string partNumber)
    {
        if (partNumber.StartsWith("HDD-00", StringComparison.OrdinalIgnoreCase))
        {
            var n = partNumber.Length >= 8 ? partNumber[7] : '1';
            return n is '3' or '4' ? "ssd" : n is '5' or '6' ? "hdd" : "storage";
        }

        return "storage";
    }

    private static string FallbackImage(int productId) =>
        productId is >= 1 and <= 6 ? $"/images/product{productId}.png" : "/images/product1.png";
}
