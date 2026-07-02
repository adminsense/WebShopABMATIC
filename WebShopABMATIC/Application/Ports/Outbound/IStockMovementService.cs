using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockMovementService
{
    /// <summary>
    /// Decrements stock for all product lines on a webshop order.
    /// Idempotent — safe to call from webhook and payment-return fallback.
    /// </summary>
    Task<StockApplyResult> ApplySaleFromOrderAsync(int orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Increments <c>ReservedQuantity</c> for PrePay checkout lines (D.7). Idempotent per order.
    /// </summary>
    Task<StockApplyResult> ApplyReservationFromOrderAsync(int orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Manual stock adjustment (+ inbound / − outbound) for a product at a specific location.
    /// </summary>
    Task<StockApplyResult> ApplyManualAdjustmentAsync(
        StockManualAdjustmentCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Moves stock between two locations (paired out/in movements).
    /// </summary>
    Task<StockApplyResult> ApplyLocationTransferAsync(
        StockLocationTransferCommand command,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Books a purchase-order delivery (GRN) and increases stock at the target location.
    /// </summary>
    Task<StockApplyResult> ApplyPurchaseOrderReceiveAsync(
        StockPoReceiveCommand command,
        CancellationToken cancellationToken = default);
}
