using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Stock;

/// <summary>
/// Evaluates the legacy <c>OrderStatus.ReserveStock</c> / <c>AffectsStock</c> flags
/// when an order's status changes via <c>OrderStructure.StatusId</c> and triggers
/// the appropriate stock operations. Ready for use when OrderStructure admin CRUD
/// is implemented.
/// </summary>
public sealed class OrderStockWorkflowService : IOrderStockWorkflowService
{
    private readonly WebShopABMATICDbContext _db;
    private readonly IStockMovementService _stock;
    private readonly ILogger<OrderStockWorkflowService> _logger;

    public OrderStockWorkflowService(
        WebShopABMATICDbContext db,
        IStockMovementService stock,
        ILogger<OrderStockWorkflowService> logger)
    {
        _db = db;
        _stock = stock;
        _logger = logger;
    }

    public async Task<StockApplyResult> OnStatusChangedAsync(
        int orderId,
        int previousStatusId,
        int newStatusId,
        CancellationToken cancellationToken = default)
    {
        if (previousStatusId == newStatusId)
        {
            return StockApplyResult.Skipped("Status unchanged.");
        }

        var statuses = await _db.OrderStatuses.AsNoTracking()
            .Where(s => s.Id == previousStatusId || s.Id == newStatusId)
            .ToDictionaryAsync(s => s.Id, cancellationToken);

        var hadReserve = statuses.TryGetValue(previousStatusId, out var prev) && prev.ReserveStock;
        var hasReserve = statuses.TryGetValue(newStatusId, out var next) && next.ReserveStock;
        var hasAffects = next?.AffectsStock == true;

        if (hasAffects && !hadReserve)
        {
            _logger.LogInformation(
                "Order {OrderId} status {PrevId}->{NewId}: AffectsStock — applying sale.",
                orderId, previousStatusId, newStatusId);
            return await _stock.ApplySaleFromOrderAsync(orderId, cancellationToken);
        }

        if (hasAffects && hadReserve)
        {
            _logger.LogInformation(
                "Order {OrderId} status {PrevId}->{NewId}: AffectsStock + was reserved — applying sale (includes release).",
                orderId, previousStatusId, newStatusId);
            return await _stock.ApplySaleFromOrderAsync(orderId, cancellationToken);
        }

        if (hasReserve && !hadReserve)
        {
            _logger.LogInformation(
                "Order {OrderId} status {PrevId}->{NewId}: ReserveStock — creating reservation.",
                orderId, previousStatusId, newStatusId);
            return await _stock.ApplyReservationFromOrderAsync(orderId, cancellationToken);
        }

        if (!hasReserve && hadReserve && !hasAffects)
        {
            _logger.LogInformation(
                "Order {OrderId} status {PrevId}->{NewId}: lost ReserveStock — releasing reservation.",
                orderId, previousStatusId, newStatusId);
            return await _stock.ReleaseReservationAsync(orderId, cancellationToken);
        }

        return StockApplyResult.Skipped("No stock-affecting status change.");
    }
}
