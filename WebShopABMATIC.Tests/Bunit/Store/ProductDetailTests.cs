using Bunit;
using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class ProductDetailTests : BunitStoreTestBase
{
    [Fact]
    public void Shows_No_description_when_empty()
    {
        AsGuest();
        Catalog.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(new StoreProductDto
            {
                Id = 1,
                Name = "Remote",
                Description = "",
                Price = 10m,
                Stock = 5,
                ImageUrl = "/x.png"
            });
        Catalog.GetProductOptionsAsync(1, Arg.Any<CancellationToken>()).Returns([]);

        var cut = RenderComponent<ProductDetail>(p => p.Add(x => x.ProductId, 1));
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("No description"));
        cut.Markup.Should().Contain("Remote");
    }

    [Fact]
    public void Disables_add_when_out_of_stock()
    {
        AsGuest();
        Catalog.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(new StoreProductDto
            {
                Id = 2,
                Name = "Gone",
                Description = "Has text",
                Price = 10m,
                Stock = 0,
                ImageUrl = "/x.png"
            });
        Catalog.GetProductOptionsAsync(2, Arg.Any<CancellationToken>()).Returns([]);

        var cut = RenderComponent<ProductDetail>(p => p.Add(x => x.ProductId, 2));
        cut.WaitForAssertion(() =>
        {
            var btn = cut.Find("button.btn-primary");
            btn.HasAttribute("disabled").Should().BeTrue();
        });
    }

    [Fact]
    public void Smoke_renders_when_product_missing()
    {
        AsGuest();
        Catalog.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((StoreProductDto?)null);
        Catalog.GetProductOptionsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns([]);
        var cut = RenderComponent<ProductDetail>(p => p.Add(x => x.ProductId, 1));
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
