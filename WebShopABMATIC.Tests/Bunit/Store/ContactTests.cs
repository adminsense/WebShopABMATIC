using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class ContactTests : BunitStoreTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsGuest();
        var cut = RenderComponent<Contact>();
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
