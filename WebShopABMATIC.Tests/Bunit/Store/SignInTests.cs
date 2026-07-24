using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class SignInTests : BunitStoreTestBase
{
    [Fact]
    public void Renders_form()
    {
        AsGuest();
        var cut = RenderComponent<SignIn>();
        cut.Markup.Should().Contain("Sign in");
        cut.FindAll("form").Should().NotBeEmpty();
    }
}
