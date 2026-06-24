using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockTransferUseCase : IStockTransferPort
{
    private readonly IStockTransferRepository _repository;
    private readonly IStockMovementService _stock;

    public StockTransferUseCase(IStockTransferRepository repository, IStockMovementService stock)
    {
        _repository = repository;
        _stock = stock;
    }

    public Task<StockTransferLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default) =>
        _repository.GetLookupsAsync(cancellationToken);

    public Task<StockTransferPreviewDto?> GetPreviewAsync(
        int productId,
        int fromStockLocationId,
        int toStockLocationId,
        CancellationToken cancellationToken = default) =>
        _repository.GetPreviewAsync(productId, fromStockLocationId, toStockLocationId, cancellationToken);

    public Task<StockApplyResult> ApplyAsync(StockTransferRequest request, CancellationToken cancellationToken = default) =>
        _stock.ApplyLocationTransferAsync(new StockLocationTransferCommand
        {
            ProductId = request.ProductId,
            FromStockLocationId = request.FromStockLocationId,
            ToStockLocationId = request.ToStockLocationId,
            Quantity = request.Quantity,
            Reason = request.Reason.Trim()
        }, cancellationToken);
}
