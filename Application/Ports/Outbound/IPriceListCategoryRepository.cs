using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.PriceListCategories;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IPriceListCategoryRepository
{
    Task<PagedResult<PriceListCategoryDto>> GetPriceListCategoriesAsync(PriceListCategoryListFilter filter, CancellationToken cancellationToken = default);
    Task<PriceListCategoryEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(PriceListCategoryEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}