using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockMovementRepository
{
    Task<PagedResult<StockMovementDto>> GetMovementsAsync(StockMovementListFilter filter, CancellationToken cancellationToken = default);
}
