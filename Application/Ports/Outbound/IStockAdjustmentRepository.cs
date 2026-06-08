using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockAdjustmentRepository
{
    Task<StockAdjustmentLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default);

    Task<StockAdjustmentPreviewDto?> GetPreviewAsync(
        int productId,
        int stockLocationId,
        CancellationToken cancellationToken = default);
}
