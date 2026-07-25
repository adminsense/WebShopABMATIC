using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.OrderStatuses;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class OrderStatusAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IOrderStatusRepository>();
        var filter = new OrderStatusListFilter();
        repo.GetOrderStatusesAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<OrderStatusDto>());
        (await new OrderStatusAdminUseCase(repo).GetOrderStatusesAsync(filter)).TotalCount.Should().Be(0);
    }
}
