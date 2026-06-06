using WebShopABMATIC.Application.Payments;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IMolliePaymentPort
{
    Task<MolliePaymentCreated> CreatePaymentAsync(CreateMolliePaymentCommand command, CancellationToken cancellationToken = default);

    Task<MolliePaymentStatusResult> GetPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default);
}
