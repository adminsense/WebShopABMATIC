namespace WebShopABMATIC.Application.Ports.Outbound;

public sealed class LegacyStaffProfileDto
{
    public required int StaffUserId { get; init; }
    public required string Login { get; init; }
    public string? Email { get; init; }
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public string? Phone { get; init; }
}

public sealed class LegacyStaffProfileUpdate
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Phone { get; init; }
}

public interface ILegacyStaffProfilePort
{
    Task<LegacyStaffProfileDto?> GetAsync(int staffUserId, CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? Error)> SaveAsync(
        int staffUserId,
        LegacyStaffProfileUpdate update,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, string? Error)> ChangePasswordAsync(
        int staffUserId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);
}
