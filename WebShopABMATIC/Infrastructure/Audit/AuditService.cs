using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class AuditService : IAuditService
{
    private readonly IAuditLogRepository _repository;
    private readonly ICurrentUserContext _currentUser;

    public AuditService(IAuditLogRepository repository, ICurrentUserContext currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task LogAsync(AuditLogWriteRequest request, CancellationToken cancellationToken = default)
    {
        var current = await _currentUser.GetCurrentUserAsync(cancellationToken);

        await _repository.AddAsync(new AuditLogWriteRequest
        {
            Action = request.Action,
            EntityName = request.EntityName,
            EntityId = request.EntityId,
            IdentityUserId = request.IdentityUserId ?? current.IdentityUserId,
            LegacyStaffUserId = request.LegacyStaffUserId ?? current.StaffUserId,
            UserDisplayName = request.UserDisplayName ?? current.AuditLabel,
            Severity = request.Severity,
            Success = request.Success,
            ErrorMessage = request.ErrorMessage,
            OldValues = request.OldValues,
            NewValues = request.NewValues,
            DurationMs = request.DurationMs,
            AdditionalInfo = request.AdditionalInfo
        }, cancellationToken);
    }
}
