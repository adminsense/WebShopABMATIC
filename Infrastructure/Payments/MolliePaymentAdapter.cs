using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Payments;

public sealed class MolliePaymentAdapter : IMolliePaymentPort
{
    private readonly IPaymentClient _paymentClient;

    public MolliePaymentAdapter(IPaymentClient paymentClient) => _paymentClient = paymentClient;

    public async Task<MolliePaymentCreated> CreatePaymentAsync(CreateMolliePaymentCommand command, CancellationToken cancellationToken = default)
    {
        var currency = string.IsNullOrWhiteSpace(command.Currency) ? Currency.EUR : command.Currency;
        var request = new PaymentRequest
        {
            Amount = new Amount(currency, command.Amount),
            Description = command.Description,
            RedirectUrl = command.RedirectUrl,
            WebhookUrl = command.WebhookUrl,
            Metadata = command.MetadataJson,
            Method = command.Method
        };

        PaymentResponse response = await _paymentClient.CreatePaymentAsync(request, cancellationToken: cancellationToken);
        return new MolliePaymentCreated
        {
            PaymentId = response.Id,
            Status = response.Status ?? "open",
            CheckoutUrl = response.Links?.Checkout?.Href
                ?? throw new InvalidOperationException("Mollie did not return a checkout URL.")
        };
    }

    public async Task<MolliePaymentStatusResult> GetPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default)
    {
        PaymentResponse response = await _paymentClient.GetPaymentAsync(molliePaymentId, cancellationToken: cancellationToken);
        return new MolliePaymentStatusResult
        {
            PaymentId = response.Id,
            Status = response.Status ?? string.Empty,
            Amount = response.Amount is null ? null : (decimal)response.Amount,
            Currency = response.Amount?.Currency
        };
    }
}
