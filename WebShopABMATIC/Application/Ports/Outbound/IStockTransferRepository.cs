using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStockTransferRepository
{
    Task<StockTransferLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default);

    Task<StockTransferPreviewDto?> GetPreviewAsync(
        int productId,
        int fromStockLocationId,
        int toStockLocationId,
        CancellationToken cancellationToken = default);
}
