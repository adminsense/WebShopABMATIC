using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockOrderRepository
{
    Task<PagedResult<StockOrderSummaryDto>> GetOrdersAsync(StockOrderListFilter filter, CancellationToken cancellationToken = default);

    Task<StockOrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);

    Task<StockOrderLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default);

    Task<int> SaveAsync(StockOrderEditDto dto, int userId, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
