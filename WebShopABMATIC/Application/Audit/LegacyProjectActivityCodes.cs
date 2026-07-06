namespace WebShopABMATIC.Application.Audit;

/// <summary>
/// Values for <c>[Logging].[ProjectActiviteit].Actie</c> (int).
/// Align with distinct codes in the legacy database when verifying.
/// </summary>
public static class LegacyProjectActivityCodes
{
    public const int WebshopCheckout = 100;
    public const int WebshopPayment = 101;
    public const int WebshopPaymentExpired = 102;
    public const int WebshopOrderCancelled = 103;
    public const int WebshopStockAdjust = 104;
    public const int CrudCreate = 10;
    public const int CrudUpdate = 11;
    public const int CrudDelete = 12;
}
