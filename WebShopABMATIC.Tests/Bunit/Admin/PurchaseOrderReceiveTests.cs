using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Bunit.Admin;

public sealed class PurchaseOrderReceiveTests : AdminPageTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsStaff();
        var cut = RenderComponent<PurchaseOrderReceive>(p => p.Add(x => x.StockOrderId, 1));
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
