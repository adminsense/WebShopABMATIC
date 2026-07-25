using Bunit;
using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class ProductListTests : AdminPageTestBase
{
    [Fact]
    public void Renders_for_staff()
    {
        AsStaff();
        ProductAdmin.GetProductsAsync(Arg.Any<ProductListFilter>(), Arg.Any<CancellationToken>())
            .Returns(new PagedResult<ProductDto>
            {
                Items = [],
                TotalCount = 0,
                Page = 1,
                PageSize = 20
            });

        var cut = RenderComponent<ProductList>();
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Product"));
    }
}
