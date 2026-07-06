using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Web.Endpoints;

public static class StockAdjustmentEndpoint
{
    public static RouteGroupBuilder MapStockAdjustmentApi(this WebApplication app)
    {
        var group = app.MapGroup("/api/admin/stock")
            .RequireAuthorization(AppPolicies.AdminOrManager);

        group.MapPost("/orders/{orderId:int}/cancel", async (
            int orderId,
            OrderCancelRequest request,
            IOrderAdminPort port,
            CancellationToken cancellationToken) =>
        {
            var result = await port.CancelOrderAsync(orderId, request.Reason ?? "Cancelled by admin", cancellationToken);
            return result.Success
                ? Results.Ok(new { result.Message, result.ReservationsReleased })
                : Results.BadRequest(new { errors = new[] { result.Message } });
        }).DisableAntiforgery();

        group.MapPost("/adjustments", async (
            StockAdjustmentRequest request,
            IStockAdjustmentPort port,
            CancellationToken cancellationToken) =>
        {
            var result = await port.ApplyAsync(request, cancellationToken);
            if (result.Status == StockApplyStatus.Failed)
            {
                return Results.BadRequest(new { errors = result.Errors });
            }

            return Results.Ok(new
            {
                status = result.Status.ToString(),
                movementId = result.MovementId,
                newBalance = result.NewBalance
            });
        }).DisableAntiforgery();

        group.MapGet("/adjustments/preview", async (
            int productId,
            int stockLocationId,
            IStockAdjustmentPort port,
            CancellationToken cancellationToken) =>
        {
            var preview = await port.GetPreviewAsync(productId, stockLocationId, cancellationToken);
            return preview is null ? Results.NotFound() : Results.Ok(preview);
        });

        group.MapPost("/transfers", async (
            StockTransferRequest request,
            IStockTransferPort port,
            CancellationToken cancellationToken) =>
        {
            var result = await port.ApplyAsync(request, cancellationToken);
            if (result.Status == StockApplyStatus.Failed)
            {
                return Results.BadRequest(new { errors = result.Errors });
            }

            return Results.Ok(new
            {
                status = result.Status.ToString(),
                outMovementId = result.OutMovementId,
                inMovementId = result.InMovementId,
                fromNewBalance = result.FromNewBalance,
                toNewBalance = result.ToNewBalance
            });
        }).DisableAntiforgery();

        group.MapGet("/transfers/preview", async (
            int productId,
            int fromStockLocationId,
            int toStockLocationId,
            IStockTransferPort port,
            CancellationToken cancellationToken) =>
        {
            var preview = await port.GetPreviewAsync(productId, fromStockLocationId, toStockLocationId, cancellationToken);
            return preview is null ? Results.NotFound() : Results.Ok(preview);
        });

        group.MapPost("/purchase-orders/{stockOrderId:int}/receive", async (
            int stockOrderId,
            StockPoReceiveRequest request,
            IStockPoReceivePort port,
            CancellationToken cancellationToken) =>
        {
            if (request.StockOrderId != stockOrderId)
            {
                return Results.BadRequest(new { errors = new[] { "Stock order id mismatch." } });
            }

            var result = await port.ApplyAsync(request, cancellationToken);
            if (result.Status == StockApplyStatus.Failed)
            {
                return Results.BadRequest(new { errors = result.Errors });
            }

            return Results.Ok(new
            {
                status = result.Status.ToString(),
                movementId = result.MovementId,
                newBalance = result.NewBalance
            });
        }).DisableAntiforgery();

        group.MapGet("/purchase-orders/{stockOrderId:int}/receive/preview", async (
            int stockOrderLineId,
            int stockLocationId,
            IStockPoReceivePort port,
            CancellationToken cancellationToken) =>
        {
            var preview = await port.GetPreviewAsync(stockOrderLineId, stockLocationId, cancellationToken);
            return preview is null ? Results.NotFound() : Results.Ok(preview);
        });

        return group;
    }
}

public sealed class OrderCancelRequest
{
    public string? Reason { get; set; }
}
