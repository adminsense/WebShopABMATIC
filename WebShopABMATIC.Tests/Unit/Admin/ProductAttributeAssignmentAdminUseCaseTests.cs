using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.ProductAttributes;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductAttributeAssignmentAdminUseCaseTests
{
    [Fact]
    public async Task SearchProducts_delegates_to_repository()
    {
        var repo = Substitute.For<IProductAttributeAssignmentRepository>();
        repo.SearchProductsAsync("Power", 1, 20, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<ProductAttributeAssignmentProductDto>());
        var result = await new ProductAttributeAssignmentAdminUseCase(repo).SearchProductsAsync("Power", 1, 20);
        result.TotalCount.Should().Be(0);
        await repo.Received(1).SearchProductsAsync("Power", 1, 20, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAssignment_delegates_to_repository()
    {
        var repo = Substitute.For<IProductAttributeAssignmentRepository>();
        repo.GetAssignmentAsync(23443, Arg.Any<CancellationToken>()).Returns((ProductAttributeAssignmentDto?)null);
        (await new ProductAttributeAssignmentAdminUseCase(repo).GetAssignmentAsync(23443)).Should().BeNull();
    }
}
