using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.ProductStockLocations;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ProductStockLocationAdminUseCase : IProductStockLocationAdminPort
{
    private readonly IProductStockLocationRepository _repository;

    public ProductStockLocationAdminUseCase(IProductStockLocationRepository repository) => _repository = repository;

    public Task<PagedResult<ProductStockLocationDto>> GetProductStockLocationsAsync(ProductStockLocationListFilter filter, CancellationToken cancellationToken = default) => _repository.GetProductStockLocationsAsync(filter, cancellationToken);
    public Task<ProductStockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(ProductStockLocationEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}