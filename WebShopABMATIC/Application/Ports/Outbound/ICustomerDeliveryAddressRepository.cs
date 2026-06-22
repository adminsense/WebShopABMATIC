using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ICustomerDeliveryAddressRepository
{
    Task<PagedResult<CustomerDeliveryAddressDto>> GetCustomerDeliveryAddressesAsync(CustomerDeliveryAddressListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerDeliveryAddressEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerDeliveryAddressEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}