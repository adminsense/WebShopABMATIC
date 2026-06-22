using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockOverviewRepository
{
    Task<StockOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default);
}
