using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockOrderAdminUseCaseTests
{
    [Fact]
    public async Task SaveAsync_passes_legacy_user_id()
    {
        var repo = Substitute.For<IStockOrderRepository>();
        var current = Substitute.For<ICurrentUserContext>();
        current.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot { IsAuthenticated = true, StaffUserId = 7 });
        var dto = new StockOrderEditDto();
        repo.SaveAsync(dto, 7, Arg.Any<CancellationToken>()).Returns(42);

        var id = await new StockOrderAdminUseCase(repo, current).SaveAsync(dto);

        id.Should().Be(42);
        await repo.Received(1).SaveAsync(dto, 7, Arg.Any<CancellationToken>());
    }
}
