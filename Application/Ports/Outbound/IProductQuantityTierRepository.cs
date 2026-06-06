using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.ProductQuantityTiers;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductQuantityTierRepository
{
    Task<PagedResult<ProductQuantityTierDto>> GetProductQuantityTiersAsync(ProductQuantityTierListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductQuantityTierEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductQuantityTierEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}