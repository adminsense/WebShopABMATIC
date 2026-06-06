using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockOverviewUseCase : IStockOverviewPort
{
    private readonly IStockOverviewRepository _repository;

    public StockOverviewUseCase(IStockOverviewRepository repository) => _repository = repository;

    public Task<StockOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default) =>
        _repository.GetOverviewAsync(cancellationToken);
}
