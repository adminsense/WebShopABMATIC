namespace WebShopABMATIC.Application.Payments;

public sealed class CreateMolliePaymentCommand
{
    public required decimal Amount { get; init; }
    public required string Currency { get; init; }
    public required string Description { get; init; }
    public required string RedirectUrl { get; init; }
    public required string WebhookUrl { get; init; }
    public string? MetadataJson { get; init; }
    public string? Method { get; init; }
}

public sealed class MolliePaymentCreated
{
    public required string PaymentId { get; init; }
    public required string Status { get; init; }
    public required string CheckoutUrl { get; init; }
}

public sealed class MolliePaymentStatusResult
{
    public required string PaymentId { get; init; }
    public required string Status { get; init; }
    public decimal? Amount { get; init; }
    public string? Currency { get; init; }
    public bool IsPaid => string.Equals(Status, "paid", StringComparison.OrdinalIgnoreCase);
}
