using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;
using WebShopABMATIC.Domain.Catalog.Products;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductAdminUseCaseTests
{
    [Fact]
    public async Task SaveAsync_creates_new_product()
    {
        var repo = Substitute.For<IProductRepository>();
        var media = Substitute.For<IProductMediaPort>();
        var current = Substitute.For<ICurrentUserContext>();
        current.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot { IsAuthenticated = true, StaffUserId = 3 });
        repo.SaveAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(55);

        var sut = new ProductAdminUseCase(repo, media, current);
        var id = await sut.SaveAsync(new ProductEditDto
        {
            ProductId = 0,
            NameEn = "Remote",
            OrderPartNumber = "R1",
            SupplierId = 1,
            ManufacturerId = 2,
            ShowOnWebshop = true,
            WebshopDescriptionNl = "desc"
        }, primaryImage: null);

        id.Should().Be(55);
        await repo.Received(1).SaveAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await media.Received(1).SetPrimaryImagePublishToWebAsync(55, true, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductsAsync_delegates_filter()
    {
        var repo = Substitute.For<IProductRepository>();
        var filter = new ProductListFilter { ShowOnWebshop = true };
        repo.GetProductsAsync(filter, Arg.Any<CancellationToken>())
            .Returns(new PagedResult<ProductDto>
            {
                Items = [new ProductDto { ProductId = 1, NameEn = "A", ShowOnWebshop = true }],
                TotalCount = 1,
                Page = 1,
                PageSize = 20
            });

        var page = await new ProductAdminUseCase(repo, Substitute.For<IProductMediaPort>(), Substitute.For<ICurrentUserContext>())
            .GetProductsAsync(filter);

        page.TotalCount.Should().Be(1);
        page.Items[0].NameEn.Should().Be("A");
    }
}
