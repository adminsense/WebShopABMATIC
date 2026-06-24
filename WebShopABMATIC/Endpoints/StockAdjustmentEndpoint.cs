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

        return group;
    }
}
