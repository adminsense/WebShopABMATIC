using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.Orders;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IOrderRepository
{
    Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default);
    Task<OrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(OrderEditDto dto, CancellationToken cancellationToken = default);
}