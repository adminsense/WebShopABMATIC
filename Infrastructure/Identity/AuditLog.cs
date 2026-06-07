namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class AuditLog
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = "";
    public string EntityName { get; set; } = "";
    public string? EntityId { get; set; }
    public string? IdentityUserId { get; set; }
    public int? LegacyStaffUserId { get; set; }
    public string UserDisplayName { get; set; } = "";
    public string Severity { get; set; } = "Information";
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public int? DurationMs { get; set; }
    public string? AdditionalInfo { get; set; }
}
