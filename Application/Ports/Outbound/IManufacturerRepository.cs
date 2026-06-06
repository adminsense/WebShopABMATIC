using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.Manufacturers;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IManufacturerRepository
{
    Task<PagedResult<ManufacturerDto>> GetManufacturersAsync(ManufacturerListFilter filter, CancellationToken cancellationToken = default);
    Task<ManufacturerEditDto?> GetForEditAsync(int manufacturerId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ManufacturerEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int manufacturerId, CancellationToken cancellationToken = default);
}