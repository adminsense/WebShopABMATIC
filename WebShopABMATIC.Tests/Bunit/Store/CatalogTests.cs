using Bunit;
using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class CatalogTests : BunitStoreTestBase
{
    [Fact]
    public void Homepage_renders_categories_heading()
    {
        AsGuest();
        Catalog.GetCategoryTreeAsync(Arg.Any<CancellationToken>()).Returns([]);
        Catalog.GetDealsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns([]);

        var cut = RenderComponent<Catalog>();
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Categories"));
    }
}
