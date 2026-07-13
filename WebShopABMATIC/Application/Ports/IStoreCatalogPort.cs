using WebShopABMATIC.Application.Store;

namespace WebShopABMATIC.Application.Ports;

public interface IStoreCatalogPort
{
    Task<IReadOnlyList<StoreCatalogCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreCategoryTreeNodeDto>> GetCategoryTreeAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreProductDto>> GetNewProductsAsync(int take, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StoreProductDto>> GetDealsAsync(int take, CancellationToken cancellationToken = default);
    /// <summary>Visible webshop products; when <paramref name="categoryId"/> is set, returns direct products on that leaf node only (CD4).</summary>
    Task<IReadOnlyList<StoreProductDto>> GetCatalogAsync(int? take = null, int? categoryId = null, CancellationToken cancellationToken = default);
    /// <summary>Server-side name search — does not load the full catalog.</summary>
    Task<IReadOnlyList<StoreProductDto>> SearchProductsAsync(string term, int take = 24, CancellationToken cancellationToken = default);
    Task<StoreProductDto?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
    /// <summary>Configurable options for a product (read-only), ordered for display.</summary>
    Task<IReadOnlyList<StoreProductOptionDto>> GetProductOptionsAsync(int productId, CancellationToken cancellationToken = default);
    /// <summary>Category header content (name + intro/outro price-list texts) for the storefront.</summary>
    Task<StoreCategoryDetailDto?> GetCategoryDetailAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<byte[]?> GetCategoryIconAsync(int categoryId, CancellationToken cancellationToken = default);
}
