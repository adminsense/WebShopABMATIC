using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.PaymentMethods;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IPaymentMethodRepository
{
    Task<PagedResult<PaymentMethodDto>> GetPaymentMethodsAsync(PaymentMethodListFilter filter, CancellationToken cancellationToken = default);
    Task<PaymentMethodEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(PaymentMethodEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}