using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class OrderAdminUseCaseTests
{
    [Fact]
    public async Task CancelOrder_not_found()
    {
        var repo = Substitute.For<IOrderRepository>();
        repo.GetForEditAsync(99, Arg.Any<CancellationToken>()).Returns((OrderEditDto?)null);

        var result = await new OrderAdminUseCase(
                repo,
                Substitute.For<IStockMovementService>(),
                Substitute.For<IAuditService>(),
                Substitute.For<IAuditLogRepository>())
            .CancelOrderAsync(99, "test");

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");
    }

    [Fact]
    public async Task CancelOrder_releases_reservation_and_saves()
    {
        var repo = Substitute.For<IOrderRepository>();
        var stock = Substitute.For<IStockMovementService>();
        var audit = Substitute.For<IAuditService>();
        var order = new OrderEditDto { Id = 5, IsAccepted = true };
        repo.GetForEditAsync(5, Arg.Any<CancellationToken>()).Returns(order);
        stock.ReleaseReservationAsync(5, Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(2));

        var result = await new OrderAdminUseCase(repo, stock, audit, Substitute.For<IAuditLogRepository>())
            .CancelOrderAsync(5, "customer request");

        result.Success.Should().BeTrue();
        result.ReservationsReleased.Should().Be(2);
        order.IsAccepted.Should().BeFalse();
        await repo.Received(1).SaveAsync(order, Arg.Any<CancellationToken>());
    }
}
