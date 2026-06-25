using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Stock;

public sealed class StockMovementService : IStockMovementService
{
    private readonly WebShopABMATICDbContext _db;
    private readonly ILowStockAlertService _lowStockAlerts;
    private readonly IAuditService _audit;
    private readonly IAuditSuppressionContext _auditSuppression;
    private readonly ILogger<StockMovementService> _logger;

    public StockMovementService(
        WebShopABMATICDbContext db,
        ILowStockAlertService lowStockAlerts,
        IAuditService audit,
        IAuditSuppressionContext auditSuppression,
        ILogger<StockMovementService> logger)
    {
        _db = db;
        _lowStockAlerts = lowStockAlerts;
        _audit = audit;
        _auditSuppression = auditSuppression;
        _logger = logger;
    }

    public async Task<StockApplyResult> ApplySaleFromOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var orderExists = await _db.Orders.AsNoTracking().AnyAsync(o => o.Id == orderId, cancellationToken);
        if (!orderExists)
        {
            return StockApplyResult.Failed([$"Order {orderId} not found."]);
        }

        var lines = await _db.OrderLines
            .Where(l => l.OrderId == orderId && l.ProductId != null)
            .ToListAsync(cancellationToken);

        if (lines.Count == 0)
        {
            return StockApplyResult.Skipped("Order has no product lines.");
        }

        var lineIds = lines.Select(l => l.Id).ToList();
        var alreadyApplied = await _db.StockMovements.AsNoTracking()
            .AnyAsync(m => m.OrderLineId != null && lineIds.Contains(m.OrderLineId.Value), cancellationToken);

        if (alreadyApplied)
        {
            return StockApplyResult.AlreadyApplied();
        }

        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

        var errors = new List<string>();
        var movements = new List<StockMovement>();
        var affectedStockRows = new List<(int ProductStockLocationId, decimal PreviousQuantity)>();
        var now = DateTime.UtcNow;

        foreach (var line in lines)
        {
            var productId = line.ProductId!.Value;
            var quantity = line.Quantity;

            if (quantity <= 0)
            {
                continue;
            }

            var stockLocation = await _db.ProductStockLocations
                .FirstOrDefaultAsync(
                    x => x.ProductId == productId && x.IsDefault && x.IsInactive != true,
                    cancellationToken);

            if (stockLocation is null)
            {
                errors.Add($"No default stock location for product {productId}.");
                continue;
            }

            if (stockLocation.Quantity < quantity)
            {
                errors.Add(
                    $"Insufficient stock for product {productId}: need {quantity}, have {stockLocation.Quantity}.");
                continue;
            }

            var previousQuantity = stockLocation.Quantity;
            stockLocation.Quantity -= quantity;

            movements.Add(new StockMovement
            {
                ProductId = productId,
                OrderLineId = line.Id,
                Quantity = -quantity,
                Timestamp = now,
                Notes = $"Webshop sale order #{orderId}",
                IsReservation = false,
                ProductStockLocatieId = stockLocation.Id
            });

            affectedStockRows.Add((stockLocation.Id, previousQuantity));
        }

        if (errors.Count > 0)
        {
            await tx.RollbackAsync(cancellationToken);
            _logger.LogWarning(
                "Stock apply failed for order {OrderId}: {Errors}",
                orderId,
                string.Join("; ", errors));
            return StockApplyResult.Failed(errors);
        }

        if (movements.Count == 0)
        {
            await tx.RollbackAsync(cancellationToken);
            return StockApplyResult.Skipped("No stock movements required.");
        }

        using (_auditSuppression.SuppressEntityTypes(nameof(StockMovement), nameof(ProductStockLocation)))
        {
            _db.StockMovements.AddRange(movements);
            await _db.SaveChangesAsync(cancellationToken);
        }

        await tx.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "Applied {Count} stock movement(s) for order {OrderId}.",
            movements.Count,
            orderId);

        await _audit.LogAsync(new AuditLogWriteRequest
        {
            Action = AuditActions.StockAdjust,
            EntityName = "Order",
            EntityId = orderId.ToString(),
            NewValues = JsonSerializer.Serialize(new
            {
                operation = "Sale",
                orderId,
                movementCount = movements.Count,
                lines = movements.Select(m => new { m.ProductId, m.Quantity, m.OrderLineId })
            })
        }, cancellationToken);

        foreach (var (productStockLocationId, previousQuantity) in affectedStockRows)
        {
            await _lowStockAlerts.EvaluateAsync(productStockLocationId, previousQuantity, cancellationToken);
        }

        return StockApplyResult.Applied(movements.Count);
    }

    public async Task<StockApplyResult> ApplyManualAdjustmentAsync(
        StockManualAdjustmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.QuantityChange == 0)
        {
            return StockApplyResult.Failed(["Quantity change cannot be zero."]);
        }

        var reason = command.Reason.Trim();
        if (reason.Length < 3)
        {
            return StockApplyResult.Failed(["Reason is required (minimum 3 characters)."]);
        }

        var productExists = await _db.Products.AsNoTracking()
            .AnyAsync(p => p.ProductId == command.ProductId, cancellationToken);
        if (!productExists)
        {
            return StockApplyResult.Failed([$"Product {command.ProductId} not found."]);
        }

        var locationExists = await _db.StockLocations.AsNoTracking()
            .AnyAsync(l => l.Id == command.StockLocationId, cancellationToken);
        if (!locationExists)
        {
            return StockApplyResult.Failed([$"Stock location {command.StockLocationId} not found."]);
        }

        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

        var stockRow = await _db.ProductStockLocations
            .FirstOrDefaultAsync(
                x => x.ProductId == command.ProductId
                     && x.StockLocationId == command.StockLocationId
                     && x.IsInactive != true,
                cancellationToken);

        if (stockRow is null)
        {
            await tx.RollbackAsync(cancellationToken);
            return StockApplyResult.Failed([
                $"No stock row for product {command.ProductId} at location {command.StockLocationId}. " +
                "Create a product-stock record first."
            ]);
        }

        var newQuantity = stockRow.Quantity + command.QuantityChange;
        if (newQuantity < 0)
        {
            await tx.RollbackAsync(cancellationToken);
            return StockApplyResult.Failed([
                $"Adjustment would make quantity negative (current {stockRow.Quantity}, change {command.QuantityChange})."
            ]);
        }

        var previousQuantity = stockRow.Quantity;
        stockRow.Quantity = newQuantity;

        var movement = new StockMovement
        {
            ProductId = command.ProductId,
            OrderLineId = null,
            Quantity = command.QuantityChange,
            Timestamp = DateTime.UtcNow,
            Notes = reason.Length > 150 ? reason[..150] : reason,
            IsReservation = false,
            ProductStockLocatieId = stockRow.Id
        };

        using (_auditSuppression.SuppressEntityTypes(nameof(StockMovement), nameof(ProductStockLocation)))
        {
            _db.StockMovements.Add(movement);
            await _db.SaveChangesAsync(cancellationToken);
        }

        await tx.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "Manual stock adjustment: product {ProductId}, location {LocationId}, change {Change}, new balance {Balance}",
            command.ProductId,
            command.StockLocationId,
            command.QuantityChange,
            newQuantity);

        await _audit.LogAsync(new AuditLogWriteRequest
        {
            Action = AuditActions.StockAdjust,
            EntityName = nameof(ProductStockLocation),
            EntityId = stockRow.Id.ToString(),
            NewValues = JsonSerializer.Serialize(new
            {
                operation = "ManualAdjustment",
                command.ProductId,
                command.StockLocationId,
                command.QuantityChange,
                newQuantity,
                reason,
                movementId = movement.Id
            })
        }, cancellationToken);

        await _lowStockAlerts.EvaluateAsync(stockRow.Id, previousQuantity, cancellationToken);

        return StockApplyResult.Applied(1, movement.Id, newQuantity);
    }

    public async Task<StockApplyResult> ApplyLocationTransferAsync(
        StockLocationTransferCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.Quantity <= 0)
        {
            return StockApplyResult.Failed(["Transfer quantity must be greater than zero."]);
        }

        if (command.FromStockLocationId == command.ToStockLocationId)
        {
            return StockApplyResult.Failed(["From and to stock locations must be different."]);
        }

        var reason = command.Reason.Trim();
        if (reason.Length < 3)
        {
            return StockApplyResult.Failed(["Reason is required (minimum 3 characters)."]);
        }

        var productExists = await _db.Products.AsNoTracking()
            .AnyAsync(p => p.ProductId == command.ProductId, cancellationToken);
        if (!productExists)
        {
            return StockApplyResult.Failed([$"Product {command.ProductId} not found."]);
        }

        var locationIds = new[] { command.FromStockLocationId, command.ToStockLocationId };
        var locationCount = await _db.StockLocations.AsNoTracking()
            .CountAsync(l => locationIds.Contains(l.Id), cancellationToken);
        if (locationCount != 2)
        {
            return StockApplyResult.Failed(["One or both stock locations were not found."]);
        }

        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

        var fromRow = await _db.ProductStockLocations
            .FirstOrDefaultAsync(
                x => x.ProductId == command.ProductId
                     && x.StockLocationId == command.FromStockLocationId
                     && x.IsInactive != true,
                cancellationToken);

        var toRow = await _db.ProductStockLocations
            .FirstOrDefaultAsync(
                x => x.ProductId == command.ProductId
                     && x.StockLocationId == command.ToStockLocationId
                     && x.IsInactive != true,
                cancellationToken);

        if (fromRow is null || toRow is null)
        {
            await tx.RollbackAsync(cancellationToken);
            return StockApplyResult.Failed([
                "Product-stock rows are required at both locations before transferring."
            ]);
        }

        if (fromRow.Quantity < command.Quantity)
        {
            await tx.RollbackAsync(cancellationToken);
            return StockApplyResult.Failed([
                $"Insufficient stock at source location (current {fromRow.Quantity}, transfer {command.Quantity})."
            ]);
        }

        var fromLocationName = await _db.StockLocations.AsNoTracking()
            .Where(l => l.Id == command.FromStockLocationId)
            .Select(l => l.Name)
            .FirstAsync(cancellationToken);
        var toLocationName = await _db.StockLocations.AsNoTracking()
            .Where(l => l.Id == command.ToStockLocationId)
            .Select(l => l.Name)
            .FirstAsync(cancellationToken);

        var fromPrevious = fromRow.Quantity;
        var toPrevious = toRow.Quantity;
        fromRow.Quantity -= command.Quantity;
        toRow.Quantity += command.Quantity;

        var now = DateTime.UtcNow;
        var outNote = TruncateNote($"Transfer to {toLocationName}: {reason}");
        var inNote = TruncateNote($"Transfer from {fromLocationName}: {reason}");

        var outMovement = new StockMovement
        {
            ProductId = command.ProductId,
            Quantity = -command.Quantity,
            Timestamp = now,
            Notes = outNote,
            IsReservation = false,
            ProductStockLocatieId = fromRow.Id
        };

        var inMovement = new StockMovement
        {
            ProductId = command.ProductId,
            Quantity = command.Quantity,
            Timestamp = now,
            Notes = inNote,
            IsReservation = false,
            ProductStockLocatieId = toRow.Id
        };

        using (_auditSuppression.SuppressEntityTypes(nameof(StockMovement), nameof(ProductStockLocation)))
        {
            _db.StockMovements.AddRange(outMovement, inMovement);
            await _db.SaveChangesAsync(cancellationToken);
        }

        await tx.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "Stock transfer: product {ProductId}, {Quantity} from location {FromId} to {ToId}",
            command.ProductId,
            command.Quantity,
            command.FromStockLocationId,
            command.ToStockLocationId);

        await _audit.LogAsync(new AuditLogWriteRequest
        {
            Action = AuditActions.StockAdjust,
            EntityName = nameof(ProductStockLocation),
            EntityId = $"{fromRow.Id}->{toRow.Id}",
            NewValues = JsonSerializer.Serialize(new
            {
                operation = "LocationTransfer",
                command.ProductId,
                command.FromStockLocationId,
                command.ToStockLocationId,
                command.Quantity,
                reason,
                outMovementId = outMovement.Id,
                inMovementId = inMovement.Id,
                fromNewBalance = fromRow.Quantity,
                toNewBalance = toRow.Quantity
            })
        }, cancellationToken);

        await _lowStockAlerts.EvaluateAsync(fromRow.Id, fromPrevious, cancellationToken);
        await _lowStockAlerts.EvaluateAsync(toRow.Id, toPrevious, cancellationToken);

        return StockApplyResult.TransferApplied(
            outMovement.Id,
            inMovement.Id,
            fromRow.Quantity,
            toRow.Quantity);
    }

    private static string TruncateNote(string note) =>
        note.Length > 150 ? note[..150] : note;
}
