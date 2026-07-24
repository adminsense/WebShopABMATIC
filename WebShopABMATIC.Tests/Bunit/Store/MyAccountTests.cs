using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class MyAccountTests : AdminPageTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsCustomer();
        var cut = RenderComponent<MyAccount>();
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
