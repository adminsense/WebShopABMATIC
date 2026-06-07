using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.Store.Orders;

namespace WebShopABMATIC.Application.Ports;

public interface ICheckoutPort
{
    Task<CheckoutOptionsDto?> GetOptionsAsync(StoreUserLookup user, CancellationToken cancellationToken = default);
    Task<CheckoutQuoteDto> BuildQuoteAsync(CheckoutQuoteRequest request, CancellationToken cancellationToken = default);
    Task<CheckoutResult> PlaceOrderAsync(CheckoutRequest request, StoreUserLookup user, CancellationToken cancellationToken = default);
    Task<CheckoutOrderSummaryDto?> GetOrderSummaryAsync(int orderId, StoreUserLookup user, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StoreOrderListItemDto>> GetCustomerOrdersAsync(StoreUserLookup user, CancellationToken cancellationToken = default);
}

public interface IMollieWebhookPort
{
    Task<bool> ProcessPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default);
}
