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
    /// Manual stock adjustment (+ inbound / − outbound) for a product at a specific location.
    /// </summary>
    Task<StockApplyResult> ApplyManualAdjustmentAsync(
        StockManualAdjustmentCommand command,
        CancellationToken cancellationToken = default);
}
