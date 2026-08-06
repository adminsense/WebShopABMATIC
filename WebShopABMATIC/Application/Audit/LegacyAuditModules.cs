namespace WebShopABMATIC.Application.Audit;

/// <summary>Values for <c>[Logging].[Error].ModuleName</c> (max 50).</summary>
public static class LegacyAuditModules
{
    public const string Auth = "Auth";
    public const string Audit = "Audit";
    public const string WebShop = "WebShop";
    public const string Checkout = "Checkout";
    public const string Mollie = "Mollie";
    public const string Stock = "Stock";
    public const string Admin = "Admin";

    public static readonly IReadOnlyList<string> All =
    [
        Auth, Audit, WebShop, Checkout, Mollie, Stock, Admin
    ];
}
