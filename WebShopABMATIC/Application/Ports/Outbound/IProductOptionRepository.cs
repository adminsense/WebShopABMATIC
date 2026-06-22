using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.ProductOptions;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductOptionRepository
{
    Task<PagedResult<ProductOptionDto>> GetProductOptionsAsync(ProductOptionListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductOptionEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductOptionEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}