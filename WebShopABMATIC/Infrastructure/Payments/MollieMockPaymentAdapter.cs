using System.Globalization;
using System.Web;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Payments;

/// <summary>
/// Simulates Mollie when <c>Mollie:UseMock</c> is true.
/// Redirects to in-app hosted checkout (<c>/checkout/mollie-mock</c>) with pre-filled demo card;
/// payment-return treats mock ids as paid.
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
        var returnUri = new Uri(command.RedirectUrl, UriKind.Absolute);
        var baseUrl = $"{returnUri.Scheme}://{returnUri.Authority}";

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["paymentId"] = paymentId;
        query["amount"] = command.Amount.ToString("F2", CultureInfo.InvariantCulture);
        query["currency"] = command.Currency;
        query["description"] = command.Description;
        query["returnUrl"] = command.RedirectUrl;

        var checkoutUrl = $"{baseUrl}/checkout/mollie-mock?{query}";

        _logger.LogInformation(
            "Mock Mollie payment {PaymentId} for {Amount} {Currency} — demo checkout at {CheckoutUrl}",
            paymentId,
            command.Amount,
            command.Currency,
            checkoutUrl);

        return Task.FromResult(new MolliePaymentCreated
        {
            PaymentId = paymentId,
            Status = "open",
            CheckoutUrl = checkoutUrl
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
            "Mollie mock mode is active (Mollie:UseMock=true, no ApiKey). Demo hosted checkout at /checkout/mollie-mock — configure real key for production E2E.");
    }
}
