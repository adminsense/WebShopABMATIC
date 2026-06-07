using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockAdjustmentUseCase : IStockAdjustmentPort
{
    private readonly IStockAdjustmentRepository _repository;
    private readonly IStockMovementService _stock;

    public StockAdjustmentUseCase(IStockAdjustmentRepository repository, IStockMovementService stock)
    {
        _repository = repository;
        _stock = stock;
    }

    public Task<StockAdjustmentLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default) =>
        _repository.GetLookupsAsync(cancellationToken);

    public Task<StockAdjustmentPreviewDto?> GetPreviewAsync(
        int productId,
        int stockLocationId,
        CancellationToken cancellationToken = default) =>
        _repository.GetPreviewAsync(productId, stockLocationId, cancellationToken);

    public Task<StockApplyResult> ApplyAsync(StockAdjustmentRequest request, CancellationToken cancellationToken = default) =>
        _stock.ApplyManualAdjustmentAsync(new StockManualAdjustmentCommand
        {
            ProductId = request.ProductId,
            StockLocationId = request.StockLocationId,
            QuantityChange = request.QuantityChange,
            Reason = request.Reason.Trim()
        }, cancellationToken);
}
