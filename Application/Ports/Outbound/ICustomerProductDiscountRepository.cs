using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ICustomerProductDiscountRepository
{
    Task<PagedResult<CustomerProductDiscountDto>> GetCustomerProductDiscountsAsync(CustomerProductDiscountListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerProductDiscountEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerProductDiscountEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}