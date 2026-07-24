using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockTransferUseCaseTests
{
    [Fact]
    public async Task ApplyAsync_maps_to_stock_service()
    {
        var stock = Substitute.For<IStockMovementService>();
        stock.ApplyLocationTransferAsync(Arg.Any<StockLocationTransferCommand>(), Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.TransferApplied(1, 2, 10, 5));

        var sut = new StockTransferUseCase(Substitute.For<IStockTransferRepository>(), stock);
        var result = await sut.ApplyAsync(new StockTransferRequest
        {
            ProductId = 1,
            FromStockLocationId = 2,
            ToStockLocationId = 3,
            Quantity = 5,
            Reason = "  move  "
        });

        result.IsSuccess.Should().BeTrue();
        await stock.Received(1).ApplyLocationTransferAsync(
            Arg.Is<StockLocationTransferCommand>(c =>
                c.ProductId == 1 && c.FromStockLocationId == 2 && c.ToStockLocationId == 3
                && c.Quantity == 5 && c.Reason == "move"),
            Arg.Any<CancellationToken>());
    }
}
