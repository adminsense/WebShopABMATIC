using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class CustomerDeliveryAddressAdminUseCase : ICustomerDeliveryAddressAdminPort
{
    private readonly ICustomerDeliveryAddressRepository _repository;

    public CustomerDeliveryAddressAdminUseCase(ICustomerDeliveryAddressRepository repository) => _repository = repository;

    public Task<PagedResult<CustomerDeliveryAddressDto>> GetCustomerDeliveryAddressesAsync(CustomerDeliveryAddressListFilter filter, CancellationToken cancellationToken = default) => _repository.GetCustomerDeliveryAddressesAsync(filter, cancellationToken);
    public Task<CustomerDeliveryAddressEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(CustomerDeliveryAddressEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}