using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.DeliveryTypes;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IDeliveryTypeRepository
{
    Task<PagedResult<DeliveryTypeDto>> GetDeliveryTypesAsync(DeliveryTypeListFilter filter, CancellationToken cancellationToken = default);
    Task<DeliveryTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(DeliveryTypeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}