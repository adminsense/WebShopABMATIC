using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Notifications;

/// <summary>
/// Logs low-stock alerts without SMTP or DB queue — for development until Phase 3b worker is configured.
/// </summary>
public sealed class MockLowStockEmailNotifier : ILowStockEmailNotifier
{
    private readonly ILogger<MockLowStockEmailNotifier> _logger;

    public MockLowStockEmailNotifier(ILogger<MockLowStockEmailNotifier> logger) => _logger = logger;

    public Task NotifyAsync(
        int productId,
        string productName,
        string stockLocationName,
        decimal quantity,
        decimal minQuantity,
        CancellationToken cancellationToken = default)
    {
        var subject = quantity <= 0
            ? $"[MOCK] Out of stock: {productName}"
            : $"[MOCK] Low stock: {productName}";

        var body = quantity <= 0
            ? $"{productName} is out of stock at {stockLocationName}."
            : $"{productName} at {stockLocationName}: {quantity} on hand (minimum {minQuantity}).";

        _logger.LogWarning(
            "Mock low-stock email (not sent, not queued): {Subject} — {Body}",
            subject,
            body);

        return Task.CompletedTask;
    }
}
