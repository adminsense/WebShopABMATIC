using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.ProductQuantityTiers;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductQuantityTierAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IProductQuantityTierRepository>();
        var filter = new ProductQuantityTierListFilter();
        repo.GetProductQuantityTiersAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<ProductQuantityTierDto>());
        (await new ProductQuantityTierAdminUseCase(repo).GetProductQuantityTiersAsync(filter))
            .TotalCount.Should().Be(0);
    }
}
