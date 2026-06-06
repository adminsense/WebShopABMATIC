using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.ProductQuantityTiers;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ProductQuantityTierAdminUseCase : IProductQuantityTierAdminPort
{
    private readonly IProductQuantityTierRepository _repository;

    public ProductQuantityTierAdminUseCase(IProductQuantityTierRepository repository) => _repository = repository;

    public Task<PagedResult<ProductQuantityTierDto>> GetProductQuantityTiersAsync(ProductQuantityTierListFilter filter, CancellationToken cancellationToken = default) => _repository.GetProductQuantityTiersAsync(filter, cancellationToken);
    public Task<ProductQuantityTierEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(ProductQuantityTierEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}