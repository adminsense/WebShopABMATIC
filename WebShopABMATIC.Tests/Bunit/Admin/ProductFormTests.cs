using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class ProductFormTests : AdminPageTestBase
{
    [Fact]
    public void Renders_redirect_shell_without_throw()
    {
        AsStaff();
        // ProductForm has no markup — navigates away in OnInitialized.
        var act = () => RenderComponent<ProductForm>(p => p.Add(x => x.ProductId, 0));
        act.Should().NotThrow();
    }
}
