using WebShopABMATIC.Application.Admin.AuditLogs;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IAuditService
{
    Task LogAsync(AuditLogWriteRequest request, CancellationToken cancellationToken = default);
}
