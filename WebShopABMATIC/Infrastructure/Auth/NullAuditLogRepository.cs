using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class NullAuditLogRepository : IAuditLogRepository
{
    public Task<PagedResult<AuditLogListItemDto>> GetPagedAsync(AuditLogListFilter filter, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PagedResult<AuditLogListItemDto>
        {
            Items = [],
            TotalCount = 0,
            Page = filter.Page,
            PageSize = filter.PageSize
        });

    public Task<AuditLogDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        Task.FromResult<AuditLogDetailDto?>(null);

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(false);

    public Task AddAsync(AuditLogWriteRequest request, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task<IReadOnlyList<OrderLogListItemDto>> GetOrderLogsAsync(int orderId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<OrderLogListItemDto>>([]);
}
