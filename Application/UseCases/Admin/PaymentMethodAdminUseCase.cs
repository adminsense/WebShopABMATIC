using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.PaymentMethods;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class PaymentMethodAdminUseCase : IPaymentMethodAdminPort
{
    private readonly IPaymentMethodRepository _repository;

    public PaymentMethodAdminUseCase(IPaymentMethodRepository repository) => _repository = repository;

    public Task<PagedResult<PaymentMethodDto>> GetPaymentMethodsAsync(PaymentMethodListFilter filter, CancellationToken cancellationToken = default) => _repository.GetPaymentMethodsAsync(filter, cancellationToken);
    public Task<PaymentMethodEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(PaymentMethodEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}