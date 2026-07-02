using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class StockPoReceiveUseCase : IStockPoReceivePort
{
    private readonly IStockPoReceiveRepository _repository;
    private readonly IStockMovementService _stock;

    public StockPoReceiveUseCase(IStockPoReceiveRepository repository, IStockMovementService stock)
    {
        _repository = repository;
        _stock = stock;
    }

    public Task<StockPoReceiveContextDto?> GetReceiveContextAsync(int stockOrderId, CancellationToken cancellationToken = default) =>
        _repository.GetReceiveContextAsync(stockOrderId, cancellationToken);

    public Task<StockPoReceivePreviewDto?> GetPreviewAsync(
        int stockOrderLineId,
        int stockLocationId,
        CancellationToken cancellationToken = default) =>
        _repository.GetPreviewAsync(stockOrderLineId, stockLocationId, cancellationToken);

    public Task<StockApplyResult> ApplyAsync(StockPoReceiveRequest request, CancellationToken cancellationToken = default) =>
        _stock.ApplyPurchaseOrderReceiveAsync(new StockPoReceiveCommand
        {
            StockOrderLineId = request.StockOrderLineId,
            StockLocationId = request.StockLocationId,
            DeliveryDocumentNumber = request.DeliveryDocumentNumber.Trim(),
            DeliveryDate = request.DeliveryDate.Date,
            Quantity = request.Quantity
        }, cancellationToken);
}
