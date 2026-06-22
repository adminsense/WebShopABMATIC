using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.ProductStockLocations;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductStockLocationRepository
{
    Task<PagedResult<ProductStockLocationDto>> GetProductStockLocationsAsync(ProductStockLocationListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductStockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductStockLocationEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}