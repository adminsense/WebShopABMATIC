using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.ProductOptions;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ProductOptionAdminUseCase : IProductOptionAdminPort
{
    private readonly IProductOptionRepository _repository;

    public ProductOptionAdminUseCase(IProductOptionRepository repository) => _repository = repository;

    public Task<PagedResult<ProductOptionDto>> GetProductOptionsAsync(ProductOptionListFilter filter, CancellationToken cancellationToken = default) => _repository.GetProductOptionsAsync(filter, cancellationToken);
    public Task<ProductOptionEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(ProductOptionEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}