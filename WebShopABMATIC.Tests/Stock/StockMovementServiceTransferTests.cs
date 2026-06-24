using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Tests.Stock;

public class StockMovementServiceTransferTests
{
    [Fact]
    public async Task ApplyLocationTransferAsync_WritesPairedMovementsAndUpdatesBalances()
    {
        var (db, service) = StockMovementServiceTestContext.Create(StockMovementServiceTestContext.SeedTransferScenario);

        var result = await service.ApplyLocationTransferAsync(new StockLocationTransferCommand
        {
            ProductId = 100,
            FromStockLocationId = 1,
            ToStockLocationId = 2,
            Quantity = 12m,
            Reason = "Replenish secondary warehouse"
        });

        Assert.Equal(StockApplyStatus.Applied, result.Status);
        Assert.Equal(2, result.MovementsCreated);
        Assert.Equal(38m, result.FromNewBalance);
        Assert.Equal(17m, result.ToNewBalance);

        var movements = await db.StockMovements.OrderBy(m => m.Id).ToListAsync();
        Assert.Equal(2, movements.Count);
        Assert.Equal(-12m, movements[0].Quantity);
        Assert.Equal(12m, movements[1].Quantity);
        Assert.Contains("Warehouse B", movements[0].Notes);
        Assert.Contains("Warehouse A", movements[1].Notes);

        var fromRow = await db.ProductStockLocations.SingleAsync(x => x.Id == 10);
        var toRow = await db.ProductStockLocations.SingleAsync(x => x.Id == 11);
        Assert.Equal(38m, fromRow.Quantity);
        Assert.Equal(17m, toRow.Quantity);
    }

    [Fact]
    public async Task ApplyLocationTransferAsync_WhenInsufficientStock_FailsWithoutChanges()
    {
        var (db, service) = StockMovementServiceTestContext.Create(StockMovementServiceTestContext.SeedTransferScenario);

        var result = await service.ApplyLocationTransferAsync(new StockLocationTransferCommand
        {
            ProductId = 100,
            FromStockLocationId = 1,
            ToStockLocationId = 2,
            Quantity = 100m,
            Reason = "Too much"
        });

        Assert.Equal(StockApplyStatus.Failed, result.Status);
        Assert.Empty(await db.StockMovements.ToListAsync());

        var fromRow = await db.ProductStockLocations.SingleAsync(x => x.Id == 10);
        Assert.Equal(50m, fromRow.Quantity);
    }

    [Fact]
    public async Task ApplyLocationTransferAsync_WhenSameLocation_Fails()
    {
        var (_, service) = StockMovementServiceTestContext.Create(StockMovementServiceTestContext.SeedTransferScenario);

        var result = await service.ApplyLocationTransferAsync(new StockLocationTransferCommand
        {
            ProductId = 100,
            FromStockLocationId = 1,
            ToStockLocationId = 1,
            Quantity = 1m,
            Reason = "Invalid route"
        });

        Assert.Equal(StockApplyStatus.Failed, result.Status);
        Assert.Contains(result.Errors, e => e.Contains("different", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ApplyManualAdjustmentAsync_StillWorksAfterClientSplit()
    {
        var (db, service) = StockMovementServiceTestContext.Create(StockMovementServiceTestContext.SeedTransferScenario);

        var result = await service.ApplyManualAdjustmentAsync(new StockManualAdjustmentCommand
        {
            ProductId = 100,
            StockLocationId = 1,
            QuantityChange = -2m,
            Reason = "Cycle count"
        });

        Assert.Equal(StockApplyStatus.Applied, result.Status);
        Assert.Equal(48m, result.NewBalance);

        var row = await db.ProductStockLocations.SingleAsync(x => x.Id == 10);
        Assert.Equal(48m, row.Quantity);
    }
}
