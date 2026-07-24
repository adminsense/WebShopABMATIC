using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.ProductPrices;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductPriceAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IProductPriceRepository>();
        var filter = new ProductPriceListFilter();
        repo.GetProductPricesAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<ProductPriceDto>());
        (await new ProductPriceAdminUseCase(repo).GetProductPricesAsync(filter)).TotalCount.Should().Be(0);
    }
}
