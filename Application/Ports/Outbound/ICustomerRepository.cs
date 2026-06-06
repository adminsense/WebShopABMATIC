using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.Customers;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ICustomerRepository
{
    Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerEditDto?> GetForEditAsync(int customerId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default);
}