using WebShopABMATIC.Application.Store;

namespace WebShopABMATIC.Application.Ports;

public interface IStoreCatalogPort
{
    Task<IReadOnlyList<StoreProductDto>> GetCatalogAsync(CancellationToken cancellationToken = default);
    Task<StoreProductDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
}
