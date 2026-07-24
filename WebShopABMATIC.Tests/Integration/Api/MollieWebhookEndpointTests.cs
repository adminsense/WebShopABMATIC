using System.Net;
using FluentAssertions;
using NSubstitute;

namespace WebShopABMATIC.Tests.Integration.Api;

public sealed class MollieWebhookEndpointTests : IClassFixture<WebShopApiFactory>
{
    private readonly WebShopApiFactory _factory;

    public MollieWebhookEndpointTests(WebShopApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Payments_webhook_returns_ok_and_invokes_port()
    {
        _factory.MollieWebhook.ProcessPaymentAsync("tr_123", Arg.Any<CancellationToken>()).Returns(true);

        var client = _factory.CreateClient();
        var content = new FormUrlEncodedContent(new Dictionary<string, string> { ["id"] = "tr_123" });
        var response = await client.PostAsync("/api/webhooks/mollie/payments", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await _factory.MollieWebhook.Received().ProcessPaymentAsync("tr_123", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Payments_webhook_bad_request_without_id()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync(
            "/api/webhooks/mollie/payments",
            new FormUrlEncodedContent(new Dictionary<string, string>()));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
