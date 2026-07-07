using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class AuditLogAdminUseCase : IAuditLogAdminPort
{
    private readonly IAuditLogRepository _repository;

    public AuditLogAdminUseCase(IAuditLogRepository repository) => _repository = repository;

    public Task<PagedResult<AuditLogListItemDto>> GetAuditTrailAsync(
        AuditLogListFilter filter,
        CancellationToken cancellationToken = default) =>
        _repository.GetPagedAsync(filter, cancellationToken);

    public Task<AuditLogDetailDto?> GetDetailAsync(long id, CancellationToken cancellationToken = default) =>
        _repository.GetByIdAsync(id, cancellationToken);
}
