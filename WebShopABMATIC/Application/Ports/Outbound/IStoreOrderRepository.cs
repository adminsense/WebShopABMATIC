using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.Store.Orders;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStoreOrderRepository
{
    Task<CheckoutOptionsDto?> GetCheckoutOptionsAsync(int customerId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, int>> GetAvailableStockAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);

    Task<StoreOrderCreated> CreateWebshopOrderAsync(StoreOrderCreateCommand command, CancellationToken cancellationToken = default);

    Task<StoreAdvancePaymentInfo?> GetAdvancePaymentByMollieIdAsync(string molliePaymentId, CancellationToken cancellationToken = default);

    Task MarkAdvancePaymentPaidAsync(int advancePaymentId, string mollieStatus, DateTime paidAt, CancellationToken cancellationToken = default);

    Task UpdateAdvancePaymentStatusAsync(int advancePaymentId, string mollieStatus, CancellationToken cancellationToken = default);

    /// <summary>Returns PrePay orders with open reservations older than <paramref name="olderThan"/>.</summary>
    Task<IReadOnlyList<ExpiredReservationInfo>> GetExpiredPrePayOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default);

    Task<CheckoutOrderSummaryDto?> GetOrderSummaryForCustomerAsync(int orderId, int customerId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, string>> GetProductNamesAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);

    Task UpdateAdvancePaymentMollieAsync(int orderId, string paymentId, string status, string checkoutUrl, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StoreOrderListItemDto>> GetOrdersForCustomerAsync(int customerId, CancellationToken cancellationToken = default);
}

public sealed class StoreOrderCreateCommand
{
    public int CustomerId { get; init; }
    public int ProjectId { get; init; }
    public int CustomerTypeId { get; init; }
    public int DeliveryTypeId { get; init; }
    public int BetaaltermijnId { get; init; }
    public int DeliveryAddressId { get; init; }
    public int PaymentMethodId { get; init; }
    public int CreatedByUserId { get; init; } = 1;
    public bool IsPrePay { get; init; }
    public decimal DeliveryFee { get; init; }
    public decimal VatPercentage { get; init; }
    public required IReadOnlyList<StoreOrderLineCreate> Lines { get; init; }
    public string? MolliePaymentId { get; init; }
    public string? MolliePaymentStatus { get; init; }
    public string? MollieCheckoutUrl { get; init; }
}

public sealed class StoreOrderLineCreate
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = "";
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotalExclVat { get; init; }
    public decimal VatAmount { get; init; }
    public decimal LineTotalInclVat { get; init; }
}

public sealed class StoreOrderCreated
{
    public int OrderId { get; init; }
    public int? OrderNumber { get; init; }
    public int? AdvancePaymentId { get; init; }
    public decimal TotalInclVat { get; init; }
}

public sealed class StoreAdvancePaymentInfo
{
    public int Id { get; init; }
    public int OrderId { get; init; }
    public string? MolliePaymentStatus { get; init; }
    public DateTime? MolliePaidAt { get; init; }
}

public sealed class ExpiredReservationInfo
{
    public int OrderId { get; init; }
    public int AdvancePaymentId { get; init; }
    public string? MolliePaymentId { get; init; }
    public string? MolliePaymentStatus { get; init; }
    public DateTime OrderCreatedAt { get; init; }
}
