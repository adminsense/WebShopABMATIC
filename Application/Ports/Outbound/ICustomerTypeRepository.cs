using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.CustomerTypes;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ICustomerTypeRepository
{
    Task<PagedResult<CustomerTypeDto>> GetCustomerTypesAsync(CustomerTypeListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerTypeEditDto?> GetForEditAsync(int klantTypeId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerTypeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int klantTypeId, CancellationToken cancellationToken = default);
}