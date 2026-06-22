using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.OrderStatuses;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IOrderStatusRepository
{
    Task<PagedResult<OrderStatusDto>> GetOrderStatusesAsync(OrderStatusListFilter filter, CancellationToken cancellationToken = default);
    Task<OrderStatusEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(OrderStatusEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}