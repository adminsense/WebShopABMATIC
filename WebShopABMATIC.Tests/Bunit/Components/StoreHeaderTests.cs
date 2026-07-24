using Bunit;
using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Web.Components.Store;

namespace WebShopABMATIC.Tests.Bunit.Components;

public sealed class StoreHeaderTests : BunitStoreTestBase
{
    [Fact]
    public void Guest_shows_Login()
    {
        AsGuest();
        Catalog.GetCategoryTreeAsync(Arg.Any<CancellationToken>()).Returns([]);

        var cut = RenderComponent<StoreHeader>();
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Login"));
    }
}
