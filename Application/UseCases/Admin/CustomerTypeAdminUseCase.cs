using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.CustomerTypes;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class CustomerTypeAdminUseCase : ICustomerTypeAdminPort
{
    private readonly ICustomerTypeRepository _repository;

    public CustomerTypeAdminUseCase(ICustomerTypeRepository repository) => _repository = repository;

    public Task<PagedResult<CustomerTypeDto>> GetCustomerTypesAsync(CustomerTypeListFilter filter, CancellationToken cancellationToken = default) => _repository.GetCustomerTypesAsync(filter, cancellationToken);
    public Task<CustomerTypeEditDto?> GetForEditAsync(int klantTypeId, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(klantTypeId, cancellationToken);
    public Task<int> SaveAsync(CustomerTypeEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int klantTypeId, CancellationToken cancellationToken = default) => _repository.DeleteAsync(klantTypeId, cancellationToken);
}