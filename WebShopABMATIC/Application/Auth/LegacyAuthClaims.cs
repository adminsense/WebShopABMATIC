namespace WebShopABMATIC.Application.Auth;

public static class LegacyAuthClaims
{
    public const string StaffUserId = "legacy_staff_id";
    public const string CustomerId = "legacy_customer_id";
    public const string DisplayName = "legacy_display_name";
    public const string Login = "legacy_login";
    /// <summary>Server-side browser session id for storefront customers.</summary>
    public const string StoreBrowserSession = "store_browser_session";
}
