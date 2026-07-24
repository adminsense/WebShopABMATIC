using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockOverviewUseCaseTests
{
    [Fact]
    public async Task GetOverview_delegates_to_repository()
    {
        var repo = Substitute.For<IStockOverviewRepository>();
        repo.GetOverviewAsync(Arg.Any<CancellationToken>())
            .Returns(new StockOverviewDto { SkusInStock = 3 });
        (await new StockOverviewUseCase(repo).GetOverviewAsync()).SkusInStock.Should().Be(3);
    }
}
