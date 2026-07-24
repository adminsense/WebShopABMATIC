using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Web.Components.Pages.Store;
using WebShopABMATIC.Web.Services;

namespace WebShopABMATIC.Tests.Bunit.Store;

public sealed class CartTests : BunitStoreTestBase
{
    [Fact]
    public void Guest_empty_shows_continue_shopping()
    {
        AsGuest();
        var cut = RenderComponent<Cart>();
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Your cart is empty"));
    }

    [Fact]
    public async Task Guest_with_lines_shows_sign_in_cta()
    {
        AsGuest();
        Catalog.GetByIdAsync(9, Arg.Any<CancellationToken>())
            .Returns(new StoreProductDto
            {
                Id = 9,
                Name = "Widget",
                Description = "d",
                Price = 5m,
                Stock = 10,
                ImageUrl = "/w.png"
            });
        Checkout.BuildQuoteAsync(Arg.Any<CheckoutQuoteRequest>(), Arg.Any<CancellationToken>())
            .Returns(new CheckoutQuoteDto
            {
                Subtotal = 5m,
                DeliveryFee = 0m,
                VatAmount = 1.05m,
                Total = 6.05m,
                Errors = []
            });

        var cart = Services.GetRequiredService<StoreCartService>();
        (await cart.AddProductAsync(9)).Should().BeTrue();

        var cut = RenderComponent<Cart>();
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Sign in to place order"));
    }

    [Fact]
    public async Task Customer_with_lines_shows_place_order()
    {
        AsCustomer();
        Catalog.GetByIdAsync(11, Arg.Any<CancellationToken>())
            .Returns(new StoreProductDto
            {
                Id = 11,
                Name = "Gadget",
                Description = "d",
                Price = 8m,
                Stock = 10,
                ImageUrl = "/g.png"
            });
        Checkout.GetOptionsAsync(Arg.Any<StoreUserLookup>(), Arg.Any<CancellationToken>())
            .Returns(new CheckoutOptionsDto());
        Checkout.BuildQuoteAsync(Arg.Any<CheckoutQuoteRequest>(), Arg.Any<CancellationToken>())
            .Returns(new CheckoutQuoteDto
            {
                Subtotal = 8m,
                DeliveryFee = 0m,
                VatAmount = 1.68m,
                Total = 9.68m,
                Errors = []
            });

        var cart = Services.GetRequiredService<StoreCartService>();
        await cart.BindToCustomerAsync(10);
        (await cart.AddProductAsync(11)).Should().BeTrue();

        var cut = RenderComponent<Cart>();
        cut.WaitForAssertion(() => cut.Markup.Should().Contain("Place order"));
    }
}
