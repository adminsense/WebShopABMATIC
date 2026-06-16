using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class NullLowStockAlertService : ILowStockAlertService
{
    public Task EvaluateAsync(int productStockLocationId, decimal previousQuantity, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(0);

    public Task<IReadOnlyList<StockLowAlertDto>> GetUnreadAlertsAsync(int limit, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<StockLowAlertDto>>([]);

    public Task MarkAllReadAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
