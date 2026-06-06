namespace WebShopABMATIC.Application.Auth;

/// <summary>Resolved identity + legacy bridge for the current HTTP request.</summary>
public sealed class CurrentUserSnapshot
{
    public static CurrentUserSnapshot Anonymous { get; } = new();

    public bool IsAuthenticated { get; init; }
    public string? IdentityUserId { get; init; }
    public int? CustomerId { get; init; }

    /// <summary>Legacy <c>Settings.StaffUsers.Id</c> when Identity email matches <c>StaffUsers.Login</c>.</summary>
    public int? StaffUserId { get; init; }

    public string DisplayName { get; init; } = "System";

    /// <summary>Short label for varchar audit columns (max 50).</summary>
    public string AuditLabel { get; init; } = "system";

    /// <summary>Legacy int FK (<c>CreatedByUserId</c>, etc.).</summary>
    public int ResolveLegacyUserId(int? fallbackUserId = null)
    {
        if (StaffUserId is > 0)
        {
            return StaffUserId.Value;
        }

        if (fallbackUserId is > 0)
        {
            return fallbackUserId.Value;
        }

        return 1;
    }
}
