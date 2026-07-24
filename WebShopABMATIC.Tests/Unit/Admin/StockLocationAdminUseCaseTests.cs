using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.StockLocations;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StockLocationAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IStockLocationRepository>();
        var filter = new StockLocationListFilter();
        repo.GetStockLocationsAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<StockLocationDto>());
        (await new StockLocationAdminUseCase(repo).GetStockLocationsAsync(filter)).TotalCount.Should().Be(0);
    }
}
