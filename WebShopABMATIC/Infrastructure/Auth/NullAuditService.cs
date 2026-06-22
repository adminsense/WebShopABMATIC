using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

/// <summary>Legacy-only mode: audit tables are not part of the domain database.</summary>
public sealed class NullAuditService : IAuditService
{
    public Task LogAsync(AuditLogWriteRequest request, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
