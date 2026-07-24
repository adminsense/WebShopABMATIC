using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockPoReceiveUseCaseTests
{
    [Fact]
    public async Task ApplyAsync_maps_trimmed_fields()
    {
        var stock = Substitute.For<IStockMovementService>();
        stock.ApplyPurchaseOrderReceiveAsync(Arg.Any<StockPoReceiveCommand>(), Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1));

        var when = new DateTime(2026, 7, 22, 15, 30, 0, DateTimeKind.Utc);
        var sut = new StockPoReceiveUseCase(Substitute.For<IStockPoReceiveRepository>(), stock);
        var result = await sut.ApplyAsync(new StockPoReceiveRequest
        {
            StockOrderLineId = 8,
            StockLocationId = 4,
            DeliveryDocumentNumber = "  DN-1  ",
            DeliveryDate = when,
            Quantity = 3
        });

        result.IsSuccess.Should().BeTrue();
        await stock.Received(1).ApplyPurchaseOrderReceiveAsync(
            Arg.Is<StockPoReceiveCommand>(c =>
                c.StockOrderLineId == 8 && c.StockLocationId == 4
                && c.DeliveryDocumentNumber == "DN-1" && c.DeliveryDate == when.Date && c.Quantity == 3),
            Arg.Any<CancellationToken>());
    }
}
