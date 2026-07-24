using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class ErrorTests : BunitStoreTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsGuest();
        var cut = RenderComponent<Error>();
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
