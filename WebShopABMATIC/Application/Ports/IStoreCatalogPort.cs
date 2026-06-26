using WebShopABMATIC.Application.Store;

namespace WebShopABMATIC.Application.Ports;

public interface IStoreCatalogPort
{
    Task<IReadOnlyList<StoreCatalogCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreCategoryTreeNodeDto>> GetCategoryTreeAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreProductDto>> GetNewProductsAsync(int take, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreProductDto>> GetDealsAsync(int take, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreProductDto>> GetCatalogAsync(int? take = null, int? categoryId = null, CancellationToken cancellationToken = default);
    Task<StoreProductDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<byte[]?> GetCategoryIconAsync(int categoryId, CancellationToken cancellationToken = default);
}
