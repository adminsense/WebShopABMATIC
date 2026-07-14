using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Store;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductPricingRepository : IProductPricingPort
{
    private readonly WebShopABMATICDbContext _db;
    private readonly StoreDbGate _dbGate;

    public ProductPricingRepository(WebShopABMATICDbContext db, StoreDbGate dbGate)
    {
        _db = db;
        _dbGate = dbGate;
    }

    public Task<decimal?> GetUnitPriceAsync(int productId, int? customerId = null, decimal quantity = 1, CancellationToken cancellationToken = default) =>
        _dbGate.RunAsync(() => ResolveUnitPriceAsync(productId, customerId, cancellationToken), cancellationToken);

    public Task<IReadOnlyDictionary<int, decimal>> GetCatalogPricesAsync(IEnumerable<int> productIds, int? customerId = null, CancellationToken cancellationToken = default) =>
        _dbGate.RunAsync(() => GetCatalogPricesCoreAsync(productIds, customerId, cancellationToken), cancellationToken);

    private async Task<IReadOnlyDictionary<int, decimal>> GetCatalogPricesCoreAsync(IEnumerable<int> productIds, int? customerId, CancellationToken cancellationToken)
    {
        var ids = productIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            return new Dictionary<int, decimal>();
        }

        if (customerId is null)
        {
            var today = DateTime.UtcNow.Date;
            var rows = await _db.ProductPrices.AsNoTracking()
                .Where(p => ids.Contains(p.ProductId)
                            && p.FromAddress.Date <= today
                            && (p.ValidTo == null || p.ValidTo.Value.Date >= today))
                .Select(p => new { p.ProductId, p.FromAddress, p.GrossSalesPrice, p.CorrectedGrossPrice })
                .ToListAsync(cancellationToken);

            return rows
                .GroupBy(p => p.ProductId)
                .Select(g =>
                {
                    var latest = g.OrderByDescending(x => x.FromAddress).First();
                    return new { g.Key, Price = ResolveListPrice(latest.GrossSalesPrice, latest.CorrectedGrossPrice) };
                })
                .Where(x => x.Price > 0)
                .ToDictionary(x => x.Key, x => x.Price);
        }

        var result = new Dictionary<int, decimal>();
        foreach (var productId in ids)
        {
            var price = await ResolveUnitPriceAsync(productId, customerId, cancellationToken);
            if (price is > 0)
            {
                result[productId] = price.Value;
            }
        }

        return result;
    }

    private async Task<decimal?> ResolveUnitPriceAsync(int productId, int? customerId, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var priceRow = await _db.ProductPrices.AsNoTracking()
            .Where(p => p.ProductId == productId
                && p.FromAddress.Date <= today
                && (p.ValidTo == null || p.ValidTo.Value.Date >= today))
            .OrderByDescending(p => p.FromAddress)
            .Select(p => new { p.GrossSalesPrice, p.CorrectedGrossPrice })
            .FirstOrDefaultAsync(cancellationToken);

        if (priceRow is null)
        {
            return null;
        }

        var listPrice = ResolveListPrice(priceRow.GrossSalesPrice, priceRow.CorrectedGrossPrice);
        if (listPrice <= 0)
        {
            return null;
        }

        if (customerId is null)
        {
            return listPrice;
        }

        var customer = await _db.Customers.AsNoTracking()
            .Where(c => c.CustomerId == customerId)
            .Select(c => new { c.CustomerTypeId })
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        {
            return listPrice;
        }

        var productDiscount = await _db.CustomerProductDiscounts.AsNoTracking()
            .Where(d => d.CustomerId == customerId
                && d.ProductId == productId
                && d.FromAddress.Date <= today
                && (d.ValidTo == null || d.ValidTo.Value.Date >= today)
                && d.DiscountPercentage != null)
            .OrderByDescending(d => d.FromAddress)
            .Select(d => d.DiscountPercentage)
            .FirstOrDefaultAsync(cancellationToken);

        var typeDiscount = await _db.CustomerTypes.AsNoTracking()
            .Where(t => t.KlantTypeId == customer.CustomerTypeId)
            .Select(t => t.BaseDiscount)
            .FirstOrDefaultAsync(cancellationToken);

        var discount = Math.Max(productDiscount ?? 0m, typeDiscount);
        if (discount <= 0)
        {
            return listPrice;
        }

        return Math.Round(listPrice * (1m - discount / 100m), 2, MidpointRounding.AwayFromZero);
    }

    private static decimal ResolveListPrice(decimal grossSalesPrice, decimal correctedGrossPrice) =>
        grossSalesPrice > 0 ? grossSalesPrice : correctedGrossPrice;
}
