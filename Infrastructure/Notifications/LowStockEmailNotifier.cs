using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Notifications;

public sealed class LowStockEmailNotifier : ILowStockEmailNotifier
{
    private const int LowStockQueueId = 2;

    private readonly WebShopABMATICDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LowStockEmailNotifier> _logger;

    public LowStockEmailNotifier(
        WebShopABMATICDbContext db,
        IConfiguration configuration,
        ILogger<LowStockEmailNotifier> logger)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task NotifyAsync(
        int productId,
        string productName,
        string stockLocationName,
        decimal quantity,
        decimal minQuantity,
        CancellationToken cancellationToken = default)
    {
        if (!_configuration.GetValue("Notifications:LowStock:Enabled", true))
        {
            return;
        }

        var recipients = _configuration["Notifications:LowStock:Recipients"]
            ?? "admin@webshop.com";

        var toList = recipients
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => x.Contains('@'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (toList.Count == 0)
        {
            return;
        }

        var subject = quantity <= 0
            ? $"Out of stock: {productName}"
            : $"Low stock: {productName}";

        var body = quantity <= 0
            ? $"{productName} is out of stock at {stockLocationName}."
            : $"{productName} at {stockLocationName}: {quantity} on hand (minimum {minQuantity}).";

        var preview = body.Length > 120 ? body[..120] : body;
        var now = DateTime.UtcNow;
        var from = _configuration["Notifications:LowStock:FromAddress"] ?? "noreply@webshop.local";

        foreach (var to in toList)
        {
            _db.EmailMessages.Add(new EmailMessage
            {
                ToAddress = to,
                FromAddress = from,
                Subject = subject,
                Body = body,
                PreviewText = preview,
                SentAt = now,
                ReceivedAt = now,
                EmailQueueId = LowStockQueueId,
                RequiresAction = true
            });
        }

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Queued low-stock email for product {ProductId} to {Recipients}",
            productId,
            string.Join(", ", toList));
    }
}
