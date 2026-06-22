using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

/// <summary>Legacy-only mode: online payments are not used.</summary>
public sealed class NullMolliePaymentPort : IMolliePaymentPort
{
    public Task<MolliePaymentCreated> CreatePaymentAsync(CreateMolliePaymentCommand command, CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException("Online payment is not available in legacy mode.");

    public Task<MolliePaymentStatusResult> GetPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException("Online payment is not available in legacy mode.");
}
