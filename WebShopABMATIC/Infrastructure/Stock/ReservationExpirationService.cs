using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Stock;

/// <summary>
/// Periodically checks for PrePay orders whose Mollie payment was never completed
/// and releases their stock reservations so the inventory isn't blocked forever.
/// </summary>
public sealed class ReservationExpirationService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan ReservationMaxAge = TimeSpan.FromMinutes(30);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReservationExpirationService> _logger;

    public ReservationExpirationService(
        IServiceScopeFactory scopeFactory,
        ILogger<ReservationExpirationService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "ReservationExpirationService started. Interval={Interval}, MaxAge={MaxAge}.",
            Interval, ReservationMaxAge);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(Interval, stoppingToken);
                await ProcessExpiredReservationsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing expired reservations.");
            }
        }
    }

    private async Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var orders = scope.ServiceProvider.GetRequiredService<IStoreOrderRepository>();
        var mollie = scope.ServiceProvider.GetRequiredService<IMolliePaymentPort>();
        var stock = scope.ServiceProvider.GetRequiredService<IStockMovementService>();

        var expired = await orders.GetExpiredPrePayOrdersAsync(ReservationMaxAge, cancellationToken);

        if (expired.Count == 0)
        {
            return;
        }

        _logger.LogInformation("Found {Count} PrePay order(s) older than {MaxAge} without payment.", expired.Count, ReservationMaxAge);

        foreach (var info in expired)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(info.MolliePaymentId))
                {
                    await stock.ReleaseReservationAsync(info.OrderId, cancellationToken);
                    await orders.UpdateAdvancePaymentStatusAsync(info.AdvancePaymentId, "expired", cancellationToken);
                    _logger.LogInformation("Released reservation for order {OrderId} (no Mollie payment ID).", info.OrderId);
                    continue;
                }

                var status = await mollie.GetPaymentAsync(info.MolliePaymentId, cancellationToken);

                if (status.IsPaid)
                {
                    continue;
                }

                var mollieStatus = status.Status.ToLowerInvariant();
                if (mollieStatus is "expired" or "canceled" or "failed")
                {
                    await stock.ReleaseReservationAsync(info.OrderId, cancellationToken);
                    await orders.UpdateAdvancePaymentStatusAsync(info.AdvancePaymentId, mollieStatus, cancellationToken);
                    _logger.LogInformation(
                        "Released reservation for order {OrderId}, Mollie status={Status}.",
                        info.OrderId, mollieStatus);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to process expired reservation for order {OrderId}.", info.OrderId);
            }
        }
    }
}
