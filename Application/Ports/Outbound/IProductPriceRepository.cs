using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.ProductPrices;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductPriceRepository
{
    Task<PagedResult<ProductPriceDto>> GetProductPricesAsync(ProductPriceListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductPriceEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductPriceEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}