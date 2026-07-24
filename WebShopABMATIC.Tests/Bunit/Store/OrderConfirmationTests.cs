using Bunit;
using FluentAssertions;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class OrderConfirmationTests : AdminPageTestBase
{
    [Fact]
    public void Renders_without_throw()
    {
        AsCustomer();
        var cut = RenderComponent<OrderConfirmation>(p => p.Add(x => x.OrderId, 1));
        cut.Markup.Should().NotBeNullOrWhiteSpace();
    }
}
