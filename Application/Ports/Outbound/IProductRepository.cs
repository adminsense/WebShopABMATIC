using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Domain.Catalog.Products;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductRepository
{
    Task<PagedResult<ProductDto>> GetProductsAsync(ProductListFilter filter, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(int productId, CancellationToken cancellationToken = default);
}
