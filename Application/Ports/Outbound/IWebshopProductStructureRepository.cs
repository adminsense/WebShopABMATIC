using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IWebshopProductStructureRepository
{
    Task<PagedResult<WebshopProductStructureDto>> GetWebshopProductStructuresAsync(WebshopProductStructureListFilter filter, CancellationToken cancellationToken = default);
    Task<WebshopProductStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(WebshopProductStructureEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}