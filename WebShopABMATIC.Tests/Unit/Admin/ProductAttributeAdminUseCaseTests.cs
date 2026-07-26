using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.ProductAttributes;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductAttributeAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IProductAttributeRepository>();
        var filter = new ProductAttributeListFilter();
        repo.GetAttributesAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<ProductAttributeDto>());
        (await new ProductAttributeAdminUseCase(repo).GetAttributesAsync(filter)).TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Delete_throws_when_attribute_has_values()
    {
        var repo = Substitute.For<IProductAttributeRepository>();
        repo.HasValuesAsync(3, Arg.Any<CancellationToken>()).Returns(true);
        var act = async () => await new ProductAttributeAdminUseCase(repo).DeleteAsync(3);
        await act.Should().ThrowAsync<InvalidOperationException>();
        await repo.DidNotReceive().DeleteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }
}
