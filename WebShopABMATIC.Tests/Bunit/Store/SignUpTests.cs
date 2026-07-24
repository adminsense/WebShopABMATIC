using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class SignUpTests : AdminPageTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsGuest();
        var cut = RenderComponent<SignUp>();
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
