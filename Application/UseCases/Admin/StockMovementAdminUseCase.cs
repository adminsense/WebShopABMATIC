using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockMovementAdminUseCase : IStockMovementAdminPort
{
    private readonly IStockMovementRepository _repository;

    public StockMovementAdminUseCase(IStockMovementRepository repository) => _repository = repository;

    public Task<PagedResult<StockMovementDto>> GetMovementsAsync(StockMovementListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetMovementsAsync(filter, cancellationToken);
}
