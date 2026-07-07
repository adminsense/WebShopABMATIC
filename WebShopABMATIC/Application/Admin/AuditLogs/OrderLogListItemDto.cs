namespace WebShopABMATIC.Application.Admin.AuditLogs;

public sealed class OrderLogListItemDto
{
    public long Id { get; init; }
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public string? UserDisplayName { get; init; }
    public string Description { get; init; } = "";
}
