using System.Text.Json;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class OrderAdminUseCase : IOrderAdminPort
{
    private readonly IOrderRepository _repository;
    private readonly IStockMovementService _stock;
    private readonly IAuditService _audit;
    private readonly IAuditLogRepository _auditLogRepository;

    public OrderAdminUseCase(
        IOrderRepository repository,
        IStockMovementService stock,
        IAuditService audit,
        IAuditLogRepository auditLogRepository)
    {
        _repository = repository;
        _stock = stock;
        _audit = audit;
        _auditLogRepository = auditLogRepository;
    }

    public Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default) => _repository.GetOrdersAsync(filter, cancellationToken);
    public Task<OrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) => _repository.GetForEditAsync(id, cancellationToken);
    public Task<int> SaveAsync(OrderEditDto dto, CancellationToken cancellationToken = default) => _repository.SaveAsync(dto, cancellationToken);

    public async Task<OrderCancelResult> CancelOrderAsync(int orderId, string reason, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetForEditAsync(orderId, cancellationToken);
        if (order is null)
        {
            return OrderCancelResult.Failed($"Order {orderId} not found.");
        }

        var releaseResult = await _stock.ReleaseReservationAsync(orderId, cancellationToken);
        var released = releaseResult.Status == StockApplyStatus.Applied ? releaseResult.MovementsCreated : 0;

        order.IsAccepted = false;
        await _repository.SaveAsync(order, cancellationToken);

        await _audit.LogAsync(new AuditLogWriteRequest
        {
            Action = AuditActions.OrderCancelled,
            EntityName = "Order",
            EntityId = orderId.ToString(),
            NewValues = JsonSerializer.Serialize(new
            {
                orderId,
                reason,
                reservationsReleased = released
            })
        }, cancellationToken);

        return OrderCancelResult.Ok(released);
    }

    public Task<IReadOnlyList<OrderLogListItemDto>> GetOrderLogsAsync(int orderId, CancellationToken cancellationToken = default) =>
        _auditLogRepository.GetOrderLogsAsync(orderId, cancellationToken);
}