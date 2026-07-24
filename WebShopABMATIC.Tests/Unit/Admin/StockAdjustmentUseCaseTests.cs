using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockAdjustmentUseCaseTests
{
    [Fact]
    public async Task ApplyAsync_forwards_to_stock_service()
    {
        var stock = Substitute.For<IStockMovementService>();
        stock.ApplyManualAdjustmentAsync(Arg.Any<StockManualAdjustmentCommand>(), Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1, movementId: 8, newBalance: 12));

        var sut = new StockAdjustmentUseCase(Substitute.For<IStockAdjustmentRepository>(), stock);
        var result = await sut.ApplyAsync(new StockAdjustmentRequest
        {
            ProductId = 1,
            StockLocationId = 2,
            QuantityChange = 3,
            Reason = "count"
        });

        result.IsSuccess.Should().BeTrue();
        result.MovementId.Should().Be(8);
        await stock.Received(1).ApplyManualAdjustmentAsync(
            Arg.Is<StockManualAdjustmentCommand>(c =>
                c.ProductId == 1 && c.StockLocationId == 2 && c.QuantityChange == 3 && c.Reason == "count"),
            Arg.Any<CancellationToken>());
    }
}
