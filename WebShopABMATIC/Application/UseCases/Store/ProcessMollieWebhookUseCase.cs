using System.Text.Json;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Store;

public sealed class ProcessMollieWebhookUseCase : IMollieWebhookPort
{
    private readonly IMolliePaymentPort _mollie;
    private readonly IStoreOrderRepository _orders;
    private readonly IStockMovementService _stock;
    private readonly IAuditService _audit;

    public ProcessMollieWebhookUseCase(
        IMolliePaymentPort mollie,
        IStoreOrderRepository orders,
        IStockMovementService stock,
        IAuditService audit)
    {
        _mollie = mollie;
        _orders = orders;
        _stock = stock;
        _audit = audit;
    }

    public async Task<bool> ProcessPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(molliePaymentId))
        {
            return false;
        }

        var existing = await _orders.GetAdvancePaymentByMollieIdAsync(molliePaymentId, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        if (existing.MolliePaidAt.HasValue ||
            string.Equals(existing.MolliePaymentStatus, "paid", StringComparison.OrdinalIgnoreCase))
        {
            await _stock.ApplySaleFromOrderAsync(existing.OrderId, cancellationToken);
            return true;
        }

        var status = await _mollie.GetPaymentAsync(molliePaymentId, cancellationToken);
        if (!status.IsPaid)
        {
            return true;
        }

        await _orders.MarkAdvancePaymentPaidAsync(existing.Id, status.Status, DateTime.UtcNow, cancellationToken);

        await _audit.LogAsync(new AuditLogWriteRequest
        {
            Action = AuditActions.PaymentPaid,
            EntityName = "Order",
            EntityId = existing.OrderId.ToString(),
            NewValues = JsonSerializer.Serialize(new
            {
                orderId = existing.OrderId,
                molliePaymentId,
                status = status.Status
            })
        }, cancellationToken);

        await _stock.ApplySaleFromOrderAsync(existing.OrderId, cancellationToken);

        return true;
    }
}
