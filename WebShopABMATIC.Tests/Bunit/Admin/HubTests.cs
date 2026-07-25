using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class HubTests : AdminPageTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsStaff();
        var cut = RenderComponent<Hub>(p => p.Add(x => x.HubId, "catalog"));
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
