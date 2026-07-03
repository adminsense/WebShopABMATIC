using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockPoReceiveRepository
{
    Task<StockPoReceiveContextDto?> GetReceiveContextAsync(int stockOrderId, CancellationToken cancellationToken = default);

    Task<StockPoReceivePreviewDto?> GetPreviewAsync(
        int stockOrderLineId,
        int stockLocationId,
        CancellationToken cancellationToken = default);
}
