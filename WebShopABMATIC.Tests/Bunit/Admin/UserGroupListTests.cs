using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class UserGroupListTests : AdminPageTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsStaff();
        var cut = RenderComponent<UserGroupList>();
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
