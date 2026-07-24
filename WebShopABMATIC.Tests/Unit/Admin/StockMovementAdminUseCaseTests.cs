using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockMovementAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IStockMovementRepository>();
        var filter = new StockMovementListFilter();
        repo.GetMovementsAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<StockMovementDto>());
        (await new StockMovementAdminUseCase(repo).GetMovementsAsync(filter)).TotalCount.Should().Be(0);
    }
}
