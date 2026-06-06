using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.WebshopStructures;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IWebshopStructureRepository
{
    Task<PagedResult<WebshopStructureDto>> GetWebshopStructuresAsync(WebshopStructureListFilter filter, CancellationToken cancellationToken = default);
    Task<WebshopStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(WebshopStructureEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}