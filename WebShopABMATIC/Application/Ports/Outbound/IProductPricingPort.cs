namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductPricingPort
{
    Task<decimal?> GetUnitPriceAsync(int productId, int? customerId = null, decimal quantity = 1, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, decimal>> GetCatalogPricesAsync(IEnumerable<int> productIds, int? customerId = null, CancellationToken cancellationToken = default);
}
