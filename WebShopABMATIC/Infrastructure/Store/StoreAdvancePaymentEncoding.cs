namespace WebShopABMATIC.Infrastructure.Store;

/// <summary>
/// Encodes Mollie correlation into existing ERP columns on
/// <c>Projecten.DossierVoorschot</c> — no invented schema.
/// <list type="bullet">
/// <item><c>Naam</c> — "Online payment|{molliePaymentId}"</item>
/// <item><c>Voorschotzichtbaarheid</c> — Mollie status (open/paid/…)</item>
/// <item><c>GefactureerdOp</c> — paid timestamp</item>
/// </list>
/// </summary>
public static class StoreAdvancePaymentEncoding
{
    public const string OnlinePaymentLabel = "Online payment";

    public static string BuildName(string? molliePaymentId) =>
        string.IsNullOrWhiteSpace(molliePaymentId)
            ? OnlinePaymentLabel
            : $"{OnlinePaymentLabel}|{molliePaymentId.Trim()}";

    public static string? ExtractMolliePaymentId(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var separator = name.LastIndexOf('|');
        if (separator < 0 || separator >= name.Length - 1)
        {
            return null;
        }

        var id = name[(separator + 1)..].Trim();
        return id.Length == 0 ? null : id;
    }

    public static bool NameContainsMolliePaymentId(string? name, string molliePaymentId) =>
        !string.IsNullOrWhiteSpace(molliePaymentId)
        && !string.IsNullOrWhiteSpace(name)
        && name.Contains(molliePaymentId, StringComparison.OrdinalIgnoreCase);
}
