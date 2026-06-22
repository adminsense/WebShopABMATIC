namespace WebShopABMATIC.Application.Store.Checkout;

public sealed class CheckoutOptionsDto
{
    public int CustomerId { get; init; }
    public IReadOnlyList<CheckoutDeliveryAddressDto> DeliveryAddresses { get; init; } = [];
    public IReadOnlyList<CheckoutPaymentMethodDto> PaymentMethods { get; init; } = [];
    public decimal DeliveryFee { get; init; }
    public decimal VatPercentage { get; init; }
}

public sealed class CheckoutDeliveryAddressDto
{
    public int Id { get; init; }
    public string Label { get; init; } = "";
}

public sealed class CheckoutPaymentMethodDto
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public bool IsPrePay { get; init; }
    public bool IsPostPay { get; init; }
}

public sealed class CheckoutLineRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}

public sealed class CheckoutQuoteRequest
{
    public required IReadOnlyList<CheckoutLineRequest> Lines { get; init; }
    public string UserEmail { get; init; } = "";
    public string? IdentityUserId { get; init; }
}

public sealed class CheckoutQuoteDto
{
    public IReadOnlyList<CheckoutLineQuoteDto> Lines { get; init; } = [];
    public decimal Subtotal { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal VatAmount { get; init; }
    public decimal Total { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}

public sealed class CheckoutLineQuoteDto
{
    public int ProductId { get; init; }
    public string Name { get; init; } = "";
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal LineTotal { get; init; }
    public int AvailableStock { get; init; }
}

public sealed class CheckoutRequest
{
    public required IReadOnlyList<CheckoutLineRequest> Lines { get; init; }
    public int DeliveryAddressId { get; init; }
    public int PaymentMethodId { get; init; }
    public string RedirectBaseUrl { get; init; } = "";
    public string WebhookBaseUrl { get; init; } = "";
}

public sealed class CheckoutResult
{
    public bool Success { get; init; }
    public int? OrderId { get; init; }
    public string? RedirectUrl { get; init; }
    public string? ConfirmationUrl { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}

public sealed class CheckoutOrderSummaryDto
{
    public int OrderId { get; init; }
    public int? OrderNumber { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal TotalInclVat { get; init; }
    public string PaymentStatus { get; init; } = "";
    public bool IsPrePay { get; init; }
    public bool IsPaid { get; init; }
    public string? MolliePaymentId { get; init; }
    public IReadOnlyList<CheckoutLineQuoteDto> Lines { get; init; } = [];
}
