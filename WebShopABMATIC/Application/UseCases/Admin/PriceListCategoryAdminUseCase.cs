using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.PriceListCategories;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class PriceListCategoryAdminUseCase : IPriceListCategoryAdminPort
{
    private readonly IPriceListCategoryRepository _repository;

    public PriceListCategoryAdminUseCase(IPriceListCategoryRepository repository) => _repository = repository;

    public Task<PagedResult<PriceListCategoryDto>> GetPriceListCategoriesAsync(PriceListCategoryListFilter filter, CancellationToken cancellationToken = default) => _repository.GetPriceListCategoriesAsync(filter, cancellationToken);
    public Task<PriceListCategoryEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(PriceListCategoryEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}