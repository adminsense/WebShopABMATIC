using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.VatTypes;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IVatTypeRepository
{
    Task<PagedResult<VatTypeDto>> GetVatTypesAsync(VatTypeListFilter filter, CancellationToken cancellationToken = default);
    Task<VatTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(VatTypeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}