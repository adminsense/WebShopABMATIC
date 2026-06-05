using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Store;

public sealed class StoreCatalogService : IStoreCatalogPort
{
    private readonly WebShopABMATICDbContext _db;
    private readonly IProductMediaPort _media;

    public StoreCatalogService(WebShopABMATICDbContext db, IProductMediaPort media)
    {
        _db = db;
        _media = media;
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

            var stock = await _db.ProductStockLocations.AsNoTracking()
                .Where(x => x.IsDefault)
                .ToDictionaryAsync(x => x.ProductId, x => (int)x.Quantity, cancellationToken);

            var items = new List<StoreProductDto>();
            foreach (var p in products)
            {
                var imageUrl = await _media.GetPrimaryImageUrlAsync(p.ProductId, webPublishedOnly: true, cancellationToken)
                    ?? FallbackImage(p.ProductId);
                items.Add(MapProduct(p.ProductId, p.NameEn, p.WebshopDescriptionNl, p.OrderPartNumber, imageUrl, stock));
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

            var stockQty = await _db.ProductStockLocations.AsNoTracking()
                .Where(x => x.ProductId == productId && x.IsDefault)
                .Select(x => (int?)x.Quantity)
                .FirstOrDefaultAsync(cancellationToken) ?? 0;

            var imageUrl = await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: true, cancellationToken)
                ?? FallbackImage(productId);

            return MapProduct(p.ProductId, p.NameEn, p.WebshopDescriptionNl, p.OrderPartNumber, imageUrl,
                new Dictionary<int, int> { [productId] = stockQty });
        }
        catch
        {
            return null;
        }
    }

    private static StoreProductDto MapProduct(
        int id,
        string name,
        string description,
        string partNumber,
        string imageUrl,
        IReadOnlyDictionary<int, int> stock)
    {
        stock.TryGetValue(id, out var qty);
        var category = InferCategory(partNumber);
        return new StoreProductDto
        {
            Id = id,
            Name = name,
            Description = description,
            ImageUrl = imageUrl,
            Price = 49.99m + ((id - 1) % 6) * 10m,
            Stock = qty,
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
