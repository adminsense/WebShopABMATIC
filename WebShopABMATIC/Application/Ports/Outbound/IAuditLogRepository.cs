using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IAuditLogRepository
{
    Task<PagedResult<AuditLogListItemDto>> GetPagedAsync(AuditLogListFilter filter, CancellationToken cancellationToken = default);
    Task<AuditLogDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderLogListItemDto>> GetOrderLogsAsync(int orderId, CancellationToken cancellationToken = default);
}
