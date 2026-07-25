using Bunit;
using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class DashboardTests : AdminPageTestBase
{
    [Fact]
    public void Renders_for_staff()
    {
        AsStaff();
        Dashboard.GetDashboardAsync(Arg.Any<CancellationToken>())
            .Returns(new AdminDashboardDto { TotalProducts = 1, ProductsOnWebshop = 1 });

        var cut = RenderComponent<Dashboard>();
        cut.WaitForAssertion(() =>
        {
            cut.Markup.Should().NotContain("Loading dashboard");
            cut.Markup.Should().NotBeNullOrWhiteSpace();
        });
    }
}
