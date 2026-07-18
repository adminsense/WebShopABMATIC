namespace WebShopABMATIC.Application.Store.Checkout;

public sealed class CheckoutOptionsDto
{
    public int CustomerId { get; init; }
    public int DeliveryTypeId { get; init; }
    public string DeliveryTypeName { get; init; } = "";
    public IReadOnlyList<CheckoutDeliveryAddressDto> DeliveryAddresses { get; init; } = [];
    public IReadOnlyList<CheckoutPaymentMethodDto> PaymentMethods { get; init; } = [];
    /// <summary>ERP freight products for the customer's delivery type (Dutch names). Empty → fee defaults to €0.</summary>
    public IReadOnlyList<CheckoutFreightOptionDto> FreightOptions { get; init; } = [];
    /// <summary>Always 0 on options load — live fee comes from selected freight product on quote.</summary>
    public decimal DeliveryFee { get; init; }
    public decimal VatPercentage { get; init; }
}

public sealed class CheckoutFreightOptionDto
{
    public int ProductId { get; init; }
    /// <summary>ERP product name (usually Dutch).</summary>
    public string Name { get; init; } = "";
    /// <summary>Valid list price from ProductPrices; 0 when missing (admin can fix later).</summary>
    public decimal UnitPrice { get; init; }
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
    /// <summary>Selectable for webshop checkout (Mollie / online PrePay). Others shown disabled.</summary>
    public bool IsSelectable { get; init; }
}

public sealed class CheckoutLineRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public IReadOnlyList<CheckoutLineOption> Options { get; init; } = [];
}

public sealed class CheckoutLineOption
{
    public int ProductOptionId { get; init; }
    public int? ProductOptionValueId { get; init; }
    public string OptionName { get; init; } = "";
    public string ValueText { get; init; } = "";
}

public sealed class CheckoutQuoteRequest
{
    public required IReadOnlyList<CheckoutLineRequest> Lines { get; init; }
    public string UserEmail { get; init; } = "";
    /// <summary>Selected ERP freight product; 0 / null → delivery fee €0.</summary>
    public int? DeliveryProductId { get; init; }
}

public sealed class CheckoutQuoteDto
{
    public IReadOnlyList<CheckoutLineQuoteDto> Lines { get; init; } = [];
    public decimal Subtotal { get; init; }
    public decimal DeliveryFee { get; init; }
    public string DeliveryLabel { get; init; } = "";
    public int? DeliveryProductId { get; init; }
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
    /// <summary>True for the selected ERP freight product persisted on the order.</summary>
    public bool IsDelivery { get; init; }
}

public sealed class CheckoutRequest
{
    public required IReadOnlyList<CheckoutLineRequest> Lines { get; init; }
    public int DeliveryAddressId { get; init; }
    public int PaymentMethodId { get; init; }
    /// <summary>Selected ERP freight product; 0 → €0 delivery.</summary>
    public int? DeliveryProductId { get; init; }
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
    public decimal TotalExclVat { get; init; }
    public decimal VatAmount { get; init; }
    public decimal TotalInclVat { get; init; }
    public string PaymentStatus { get; init; } = "";
    public bool IsPrePay { get; init; }
    public bool IsPaid { get; init; }
    public string? MolliePaymentId { get; init; }
    public IReadOnlyList<CheckoutLineQuoteDto> Lines { get; init; } = [];
}
