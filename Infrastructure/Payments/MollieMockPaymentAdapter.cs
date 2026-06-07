using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Payments;

/// <summary>
/// Simulates Mollie create/get payment when <c>Mollie:ApiKey</c> is not set.
/// Checkout redirects straight to <c>RedirectUrl</c>; payment-return treats mock ids as paid.
/// </summary>
public sealed class MollieMockPaymentAdapter : IMolliePaymentPort
{
    public const string PaymentIdPrefix = "tr_mock_";

    private readonly ILogger<MollieMockPaymentAdapter> _logger;
    private bool _loggedOnce;

    public MollieMockPaymentAdapter(ILogger<MollieMockPaymentAdapter> logger) => _logger = logger;

    public Task<MolliePaymentCreated> CreatePaymentAsync(CreateMolliePaymentCommand command, CancellationToken cancellationToken = default)
    {
        LogMockModeOnce();

        var paymentId = $"{PaymentIdPrefix}{Guid.NewGuid():N}";

        _logger.LogInformation(
            "Mock Mollie payment {PaymentId} for {Amount} {Currency} — redirecting to {RedirectUrl}",
            paymentId,
            command.Amount,
            command.Currency,
            command.RedirectUrl);

        return Task.FromResult(new MolliePaymentCreated
        {
            PaymentId = paymentId,
            Status = "open",
            CheckoutUrl = command.RedirectUrl
        });
    }

    public Task<MolliePaymentStatusResult> GetPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default)
    {
        if (!IsMockPaymentId(molliePaymentId))
        {
            throw new InvalidOperationException(
                $"Payment id '{molliePaymentId}' is not a mock Mollie id. Configure Mollie:ApiKey for live payments.");
        }

        _logger.LogDebug("Mock Mollie GetPayment {PaymentId} → paid", molliePaymentId);

        return Task.FromResult(new MolliePaymentStatusResult
        {
            PaymentId = molliePaymentId,
            Status = "paid",
            Amount = null,
            Currency = "EUR"
        });
    }

    public static bool IsMockPaymentId(string? paymentId) =>
        !string.IsNullOrWhiteSpace(paymentId)
        && paymentId.StartsWith(PaymentIdPrefix, StringComparison.OrdinalIgnoreCase);

    private void LogMockModeOnce()
    {
        if (_loggedOnce)
        {
            return;
        }

        _loggedOnce = true;
        _logger.LogWarning(
            "Mollie mock mode is active (Mollie:UseMock=true, no ApiKey). PrePay checkout simulates paid status — configure real key for Phase 5 E2E.");
    }
}
