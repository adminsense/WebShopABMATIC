using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Store;

public sealed class ProcessMollieWebhookUseCase : IMollieWebhookPort
{
    private readonly IMolliePaymentPort _mollie;
    private readonly IStoreOrderRepository _orders;

    public ProcessMollieWebhookUseCase(IMolliePaymentPort mollie, IStoreOrderRepository orders)
    {
        _mollie = mollie;
        _orders = orders;
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
            return true;
        }

        var status = await _mollie.GetPaymentAsync(molliePaymentId, cancellationToken);
        if (!status.IsPaid)
        {
            return true;
        }

        await _orders.MarkAdvancePaymentPaidAsync(existing.Id, status.Status, DateTime.UtcNow, cancellationToken);
        return true;
    }
}
