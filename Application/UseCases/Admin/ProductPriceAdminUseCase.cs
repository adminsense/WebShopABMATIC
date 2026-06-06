using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.ProductPrices;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ProductPriceAdminUseCase : IProductPriceAdminPort
{
    private readonly IProductPriceRepository _repository;

    public ProductPriceAdminUseCase(IProductPriceRepository repository) => _repository = repository;

    public Task<PagedResult<ProductPriceDto>> GetProductPricesAsync(ProductPriceListFilter filter, CancellationToken cancellationToken = default) => _repository.GetProductPricesAsync(filter, cancellationToken);
    public Task<ProductPriceEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(ProductPriceEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}