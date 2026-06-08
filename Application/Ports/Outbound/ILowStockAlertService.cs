namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ILowStockAlertService
{
    /// <summary>
    /// Creates an in-app alert when stock is at or below minimum (on threshold cross or while still low without unread alert).
    /// </summary>
    Task EvaluateAsync(int productStockLocationId, decimal previousQuantity, CancellationToken cancellationToken = default);

    Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Application.Admin.Stock.StockLowAlertDto>> GetUnreadAlertsAsync(
        int limit,
        CancellationToken cancellationToken = default);

    Task MarkAllReadAsync(CancellationToken cancellationToken = default);
}
