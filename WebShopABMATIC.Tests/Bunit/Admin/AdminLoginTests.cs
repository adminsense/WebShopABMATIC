using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class AdminLoginTests : BunitStoreTestBase
{
    [Fact]
    public void Renders_form()
    {
        AsGuest();
        var cut = RenderComponent<AdminLogin>();
        cut.Markup.Should().Contain("Staff log in");
    }
}
