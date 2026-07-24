using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.ProductStockLocations;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductStockLocationAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IProductStockLocationRepository>();
        var filter = new ProductStockLocationListFilter();
        repo.GetProductStockLocationsAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<ProductStockLocationDto>());
        (await new ProductStockLocationAdminUseCase(repo).GetProductStockLocationsAsync(filter))
            .TotalCount.Should().Be(0);
    }
}
