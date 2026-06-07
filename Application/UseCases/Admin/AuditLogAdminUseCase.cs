using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class AuditLogAdminUseCase : IAuditLogAdminPort
{
    private readonly IAuditLogRepository _repository;

    public AuditLogAdminUseCase(IAuditLogRepository repository) => _repository = repository;

    public Task<PagedResult<AuditLogListItemDto>> GetAuditLogsAsync(AuditLogListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetPagedAsync(filter, cancellationToken);

    public Task<AuditLogDetailDto?> GetAuditLogDetailAsync(long id, CancellationToken cancellationToken = default) =>
        _repository.GetByIdAsync(id, cancellationToken);
}
