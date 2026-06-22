using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.AuditLogs;

using WebShopABMATIC.Application.Audit;

public sealed class AuditLogListItemDto
{
    public long Id { get; init; }
    public DateTime Timestamp { get; init; }
    public string Action { get; init; } = "";
    public string EntityName { get; init; } = "";
    public string? EntityId { get; init; }
    public string UserDisplayName { get; init; } = "";
    public string Severity { get; init; } = "";
    public bool Success { get; init; }
}

public sealed class AuditLogDetailDto
{
    public long Id { get; init; }
    public DateTime Timestamp { get; init; }
    public string Action { get; init; } = "";
    public string EntityName { get; init; } = "";
    public string? EntityId { get; init; }
    public string? IdentityUserId { get; init; }
    public int? LegacyStaffUserId { get; init; }
    public string UserDisplayName { get; init; } = "";
    public string Severity { get; init; } = "";
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
    public int? DurationMs { get; init; }
    public string? AdditionalInfo { get; init; }
}

public sealed class AuditLogListFilter
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Action { get; set; }
    public string? Severity { get; set; }
    public string? UserSearch { get; set; }
    public string? EntityName { get; set; }
    public bool? SuccessOnly { get; set; }
    public bool? FailuresOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}

public sealed class AuditLogWriteRequest
{
    public required string Action { get; init; }
    public required string EntityName { get; init; }
    public string? EntityId { get; init; }
    public string? IdentityUserId { get; init; }
    public int? LegacyStaffUserId { get; init; }
    public string? UserDisplayName { get; init; }
    public string Severity { get; init; } = AuditSeverity.Information;
    public bool Success { get; init; } = true;
    public string? ErrorMessage { get; init; }
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
    public int? DurationMs { get; init; }
    public string? AdditionalInfo { get; init; }
}
