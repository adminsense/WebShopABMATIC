using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.Suppliers;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ISupplierRepository
{
    Task<PagedResult<SupplierDto>> GetSuppliersAsync(SupplierListFilter filter, CancellationToken cancellationToken = default);
    Task<SupplierEditDto?> GetForEditAsync(int supplierId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(SupplierEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int supplierId, CancellationToken cancellationToken = default);
}