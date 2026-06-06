using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class CustomerProductDiscountAdminUseCase : ICustomerProductDiscountAdminPort
{
    private readonly ICustomerProductDiscountRepository _repository;

    public CustomerProductDiscountAdminUseCase(ICustomerProductDiscountRepository repository) => _repository = repository;

    public Task<PagedResult<CustomerProductDiscountDto>> GetCustomerProductDiscountsAsync(CustomerProductDiscountListFilter filter, CancellationToken cancellationToken = default) => _repository.GetCustomerProductDiscountsAsync(filter, cancellationToken);
    public Task<CustomerProductDiscountEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(CustomerProductDiscountEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}