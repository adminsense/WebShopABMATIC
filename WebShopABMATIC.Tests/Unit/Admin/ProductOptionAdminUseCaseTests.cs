using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.ProductOptions;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductOptionAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IProductOptionRepository>();
        var filter = new ProductOptionListFilter();
        repo.GetProductOptionsAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<ProductOptionDto>());
        (await new ProductOptionAdminUseCase(repo).GetProductOptionsAsync(filter)).TotalCount.Should().Be(0);
    }
}
