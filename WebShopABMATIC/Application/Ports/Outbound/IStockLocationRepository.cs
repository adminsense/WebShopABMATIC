using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.StockLocations;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockLocationRepository
{
    Task<PagedResult<StockLocationDto>> GetStockLocationsAsync(StockLocationListFilter filter, CancellationToken cancellationToken = default);
    Task<StockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(StockLocationEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}