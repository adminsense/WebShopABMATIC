using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Application.Ports.Outbound;

/// <summary>
/// Evaluates legacy <c>OrderStatus.ReserveStock</c> / <c>AffectsStock</c> flags
/// on status transitions and triggers the appropriate stock operations.
/// </summary>
public interface IOrderStockWorkflowService
{
    Task<StockApplyResult> OnStatusChangedAsync(
        int orderId,
        int previousStatusId,
        int newStatusId,
        CancellationToken cancellationToken = default);
}
