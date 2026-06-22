namespace WebShopABMATIC.Application.Store.Orders;

public sealed class StoreOrderListItemDto
{
    public int OrderId { get; init; }
    public int? OrderNumber { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal TotalInclVat { get; init; }
    public string PaymentStatus { get; init; } = "";
    public bool IsPrePay { get; init; }
    public bool IsPaid { get; init; }
    public string ItemsSummary { get; init; } = "";
    public string StatusLabel { get; init; } = "";
    public string StatusClass { get; init; } = "";
}

public static class StoreOrderPaymentDisplay
{
    public static (string Label, string CssClass) Describe(bool isPrePay, bool isPaid, string? paymentStatus)
    {
        if (!isPrePay)
        {
            return ("Invoice", "invoice");
        }

        if (isPaid || string.Equals(paymentStatus, "paid", StringComparison.OrdinalIgnoreCase))
        {
            return ("Paid", "paid");
        }

        return paymentStatus?.ToLowerInvariant() switch
        {
            "open" => ("Awaiting payment", "pending"),
            "canceled" or "cancelled" => ("Cancelled", "cancelled"),
            "expired" => ("Expired", "cancelled"),
            "failed" => ("Failed", "cancelled"),
            _ => ("Pending payment", "pending")
        };
    }
}
