using System.Text.Json;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Audit;

public static class AuditManualLogger
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = false };

    public static Task LogIdentityUserAsync(
        IAuditService audit,
        string action,
        string userId,
        string? email,
        object? payload,
        CancellationToken cancellationToken = default) =>
        audit.LogAsync(new AuditLogWriteRequest
        {
            Action = action,
            EntityName = "ApplicationUser",
            EntityId = userId,
            IdentityUserId = userId,
            UserDisplayName = email,
            NewValues = payload is null ? null : JsonSerializer.Serialize(payload, JsonOptions)
        }, cancellationToken);

}
