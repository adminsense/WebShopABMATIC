using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.UseCases.Store;

namespace WebShopABMATIC.Tests.Unit.Store;

public sealed class ProcessMollieWebhookUseCaseTests
{
    private readonly IMolliePaymentPort _mollie = Substitute.For<IMolliePaymentPort>();
    private readonly IStoreOrderRepository _orders = Substitute.For<IStoreOrderRepository>();
    private readonly IStockMovementService _stock = Substitute.For<IStockMovementService>();
    private readonly IAuditService _audit = Substitute.For<IAuditService>();

    private ProcessMollieWebhookUseCase CreateSut() => new(_mollie, _orders, _stock, _audit);

    [Fact]
    public async Task ProcessPayment_returns_false_for_blank_id()
    {
        var ok = await CreateSut().ProcessPaymentAsync(" ");
        ok.Should().BeFalse();
    }

    [Fact]
    public async Task ProcessPayment_returns_false_when_advance_missing()
    {
        _orders.GetAdvancePaymentByMollieIdAsync("tr_x", Arg.Any<CancellationToken>())
            .Returns((StoreAdvancePaymentInfo?)null);

        var ok = await CreateSut().ProcessPaymentAsync("tr_x");
        ok.Should().BeFalse();
    }

    [Fact]
    public async Task ProcessPayment_already_paid_applies_sale()
    {
        _orders.GetAdvancePaymentByMollieIdAsync("tr_paid", Arg.Any<CancellationToken>())
            .Returns(new StoreAdvancePaymentInfo
            {
                Id = 1,
                OrderId = 9,
                MolliePaymentStatus = "paid",
                MolliePaidAt = DateTime.UtcNow
            });
        _stock.ApplySaleFromOrderAsync(9, Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1));

        var ok = await CreateSut().ProcessPaymentAsync("tr_paid");

        ok.Should().BeTrue();
        await _stock.Received(1).ApplySaleFromOrderAsync(9, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessPayment_canceled_releases_reservation()
    {
        _orders.GetAdvancePaymentByMollieIdAsync("tr_c", Arg.Any<CancellationToken>())
            .Returns(new StoreAdvancePaymentInfo { Id = 2, OrderId = 11, MolliePaymentStatus = "open" });
        _mollie.GetPaymentAsync("tr_c", Arg.Any<CancellationToken>())
            .Returns(new MolliePaymentStatusResult { PaymentId = "tr_c", Status = "canceled" });
        _stock.ReleaseReservationAsync(11, Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1));

        var ok = await CreateSut().ProcessPaymentAsync("tr_c");

        ok.Should().BeTrue();
        await _orders.Received(1).UpdateAdvancePaymentStatusAsync(2, "canceled", Arg.Any<CancellationToken>());
        await _stock.Received(1).ReleaseReservationAsync(11, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessPayment_paid_marks_and_audits()
    {
        _orders.GetAdvancePaymentByMollieIdAsync("tr_new", Arg.Any<CancellationToken>())
            .Returns(new StoreAdvancePaymentInfo { Id = 3, OrderId = 15, MolliePaymentStatus = "open" });
        _mollie.GetPaymentAsync("tr_new", Arg.Any<CancellationToken>())
            .Returns(new MolliePaymentStatusResult { PaymentId = "tr_new", Status = "paid" });
        _stock.ApplySaleFromOrderAsync(15, Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1));

        var ok = await CreateSut().ProcessPaymentAsync("tr_new");

        ok.Should().BeTrue();
        await _orders.Received(1).MarkAdvancePaymentPaidAsync(
            3, "paid", Arg.Any<DateTime>(), Arg.Any<CancellationToken>());
        await _stock.Received(1).ApplySaleFromOrderAsync(15, Arg.Any<CancellationToken>());
        await _audit.Received(1).LogAsync(Arg.Any<WebShopABMATIC.Application.Admin.AuditLogs.AuditLogWriteRequest>(), Arg.Any<CancellationToken>());
    }
}
