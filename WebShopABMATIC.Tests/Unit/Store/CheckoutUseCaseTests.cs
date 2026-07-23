using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.UseCases.Store;

namespace WebShopABMATIC.Tests.Unit.Store;

public sealed class CheckoutUseCaseTests
{
    private readonly IStoreCustomerRepository _customers = Substitute.For<IStoreCustomerRepository>();
    private readonly IStoreOrderRepository _orders = Substitute.For<IStoreOrderRepository>();
    private readonly IStoreCatalogPort _catalog = Substitute.For<IStoreCatalogPort>();
    private readonly IProductPricingPort _pricing = Substitute.For<IProductPricingPort>();
    private readonly IMolliePaymentPort _mollie = Substitute.For<IMolliePaymentPort>();
    private readonly IMollieWebhookPort _webhook = Substitute.For<IMollieWebhookPort>();
    private readonly IStockMovementService _stock = Substitute.For<IStockMovementService>();
    private readonly ICurrentUserContext _currentUser = Substitute.For<ICurrentUserContext>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();

    private CheckoutUseCase CreateSut() =>
        new(_customers, _orders, _catalog, _pricing, _mollie, _webhook, _stock, _currentUser, _audit);

    [Fact]
    public async Task BuildQuote_empty_cart_returns_error()
    {
        var sut = CreateSut();
        var quote = await sut.BuildQuoteAsync(new CheckoutQuoteRequest { Lines = [] });

        quote.Errors.Should().ContainSingle(e => e.Contains("empty", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task BuildQuote_unlinked_customer_returns_error()
    {
        _currentUser.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot { IsAuthenticated = true, CustomerId = 10 });
        _customers.GetForStoreUserAsync(Arg.Any<StoreUserLookup>(), Arg.Any<CancellationToken>())
            .Returns((StoreCustomerContext?)null);

        var sut = CreateSut();
        var quote = await sut.BuildQuoteAsync(new CheckoutQuoteRequest
        {
            Lines = [new CheckoutLineRequest { ProductId = 1, Quantity = 1 }]
        });

        quote.Errors.Should().Contain(e => e.Contains("not linked", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task BuildQuote_applies_freight_price_and_vat()
    {
        SetupCustomerQuoteBasics(unitPrice: 100m, stock: 5, freightProductId: 900, freightPrice: 10m);

        var sut = CreateSut();
        var quote = await sut.BuildQuoteAsync(new CheckoutQuoteRequest
        {
            Lines = [new CheckoutLineRequest { ProductId = 1, Quantity = 2 }],
            DeliveryProductId = 900
        });

        quote.Errors.Should().BeEmpty();
        quote.Subtotal.Should().Be(200m);
        quote.DeliveryFee.Should().Be(10m);
        quote.VatAmount.Should().Be(44.10m); // (200+10)*0.21
        quote.Total.Should().Be(254.10m);
    }

    [Fact]
    public async Task BuildQuote_missing_freight_price_is_zero()
    {
        SetupCustomerQuoteBasics(unitPrice: 50m, stock: 2, freightProductId: 901, freightPrice: 0m);

        var sut = CreateSut();
        var quote = await sut.BuildQuoteAsync(new CheckoutQuoteRequest
        {
            Lines = [new CheckoutLineRequest { ProductId = 1, Quantity = 1 }],
            DeliveryProductId = 901
        });

        quote.DeliveryFee.Should().Be(0m);
        quote.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task PlaceOrder_fails_when_customer_missing()
    {
        _currentUser.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(CurrentUserSnapshot.Anonymous);
        _customers.GetForStoreUserAsync(Arg.Any<StoreUserLookup>(), Arg.Any<CancellationToken>())
            .Returns((StoreCustomerContext?)null);

        var sut = CreateSut();
        var result = await sut.PlaceOrderAsync(
            new CheckoutRequest
            {
                Lines = [new CheckoutLineRequest { ProductId = 1, Quantity = 1 }],
                DeliveryAddressId = 1,
                PaymentMethodId = 1
            },
            new StoreUserLookup { Email = "guest@example.com" });

        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PlaceOrder_prepay_creates_order_reserves_and_starts_mollie()
    {
        SetupCustomerQuoteBasics(unitPrice: 20m, stock: 10, freightProductId: null, freightPrice: 0m);
        _orders.GetCheckoutOptionsAsync(1, Arg.Any<CancellationToken>()).Returns(new CheckoutOptionsDto
        {
            CustomerId = 1,
            DeliveryAddresses = [new CheckoutDeliveryAddressDto { Id = 5, Label = "Home" }],
            PaymentMethods =
            [
                new CheckoutPaymentMethodDto
                {
                    Id = 7,
                    Name = "Bancontact",
                    IsPrePay = true,
                    IsSelectable = true
                }
            ],
            FreightOptions = []
        });
        _orders.CreateWebshopOrderAsync(Arg.Any<StoreOrderCreateCommand>(), Arg.Any<CancellationToken>())
            .Returns(new StoreOrderCreated
            {
                OrderId = 42,
                OrderNumber = 1001,
                AdvancePaymentId = 3,
                TotalInclVat = 24.20m
            });
        _stock.ApplyReservationFromOrderAsync(42, Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1));
        _mollie.CreatePaymentAsync(Arg.Any<CreateMolliePaymentCommand>(), Arg.Any<CancellationToken>())
            .Returns(new MolliePaymentCreated
            {
                PaymentId = "tr_test",
                Status = "open",
                CheckoutUrl = "https://mock/pay"
            });

        var sut = CreateSut();
        var result = await sut.PlaceOrderAsync(
            new CheckoutRequest
            {
                Lines = [new CheckoutLineRequest { ProductId = 1, Quantity = 1 }],
                DeliveryAddressId = 5,
                PaymentMethodId = 7,
                RedirectBaseUrl = "https://shop.test",
                WebhookBaseUrl = "https://shop.test"
            },
            new StoreUserLookup { CustomerId = 1, Email = "c@test.com" });

        result.Success.Should().BeTrue();
        result.OrderId.Should().Be(42);
        result.RedirectUrl.Should().Be("https://mock/pay");
        await _stock.Received(1).ApplyReservationFromOrderAsync(42, Arg.Any<CancellationToken>());
        await _mollie.Received(1).CreatePaymentAsync(Arg.Any<CreateMolliePaymentCommand>(), Arg.Any<CancellationToken>());
    }

    private void SetupCustomerQuoteBasics(decimal unitPrice, int stock, int? freightProductId, decimal freightPrice)
    {
        _currentUser.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot { IsAuthenticated = true, CustomerId = 1 });
        _customers.GetForStoreUserAsync(Arg.Any<StoreUserLookup>(), Arg.Any<CancellationToken>())
            .Returns(new StoreCustomerContext
            {
                CustomerId = 1,
                CustomerTypeId = 1,
                DeliveryTypeId = 1,
                BetaaltermijnId = 1,
                ProjectId = 1,
                AccountManagerUserId = 1
            });
        _pricing.GetCatalogPricesAsync(Arg.Any<IReadOnlyList<int>>(), 1, Arg.Any<CancellationToken>())
            .Returns(new Dictionary<int, decimal> { [1] = unitPrice });
        _orders.GetAvailableStockAsync(Arg.Any<IEnumerable<int>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<int, int> { [1] = stock });
        _orders.GetProductNamesAsync(Arg.Any<IEnumerable<int>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<int, string> { [1] = "Widget" });
        _catalog.GetProductOptionsAsync(1, Arg.Any<CancellationToken>()).Returns([]);

        var freight = freightProductId is int id
            ?
            [
                new CheckoutFreightOptionDto
                {
                    ProductId = id,
                    Name = "Freight",
                    UnitPrice = freightPrice
                }
            ]
            : Array.Empty<CheckoutFreightOptionDto>();

        _orders.GetCheckoutOptionsAsync(1, Arg.Any<CancellationToken>()).Returns(new CheckoutOptionsDto
        {
            CustomerId = 1,
            FreightOptions = freight,
            DeliveryAddresses = [new CheckoutDeliveryAddressDto { Id = 5, Label = "Home" }],
            PaymentMethods =
            [
                new CheckoutPaymentMethodDto
                {
                    Id = 7,
                    Name = "Bancontact",
                    IsPrePay = true,
                    IsSelectable = true
                }
            ]
        });
    }
}
