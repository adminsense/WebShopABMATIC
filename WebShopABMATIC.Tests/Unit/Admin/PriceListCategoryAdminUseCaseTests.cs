using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.PriceListCategories;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class PriceListCategoryAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IPriceListCategoryRepository>();
        var filter = new PriceListCategoryListFilter();
        repo.GetPriceListCategoriesAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<PriceListCategoryDto>());
        (await new PriceListCategoryAdminUseCase(repo).GetPriceListCategoriesAsync(filter))
            .TotalCount.Should().Be(0);
    }
}
