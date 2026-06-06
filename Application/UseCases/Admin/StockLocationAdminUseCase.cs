using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.StockLocations;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockLocationAdminUseCase : IStockLocationAdminPort
{
    private readonly IStockLocationRepository _repository;

    public StockLocationAdminUseCase(IStockLocationRepository repository) => _repository = repository;

    public Task<PagedResult<StockLocationDto>> GetStockLocationsAsync(StockLocationListFilter filter, CancellationToken cancellationToken = default) => _repository.GetStockLocationsAsync(filter, cancellationToken);
    public Task<StockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(StockLocationEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) => _repository.DeleteAsync(id, cancellationToken);
}