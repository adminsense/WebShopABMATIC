using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Stock;

namespace WebShopABMATIC.Tests.Integration.Api;

public sealed class StockAdjustmentEndpointTests : IClassFixture<WebShopApiFactory>
{
    private readonly WebShopApiFactory _factory;

    public StockAdjustmentEndpointTests(WebShopApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Adjustments_unauthorized_without_auth()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.PostAsJsonAsync("/api/admin/stock/adjustments", new StockAdjustmentRequest
        {
            ProductId = 1,
            StockLocationId = 1,
            QuantityChange = 1,
            Reason = "test"
        });

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Unauthorized, HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Adjustments_ok_for_manager()
    {
        _factory.StockAdjustment.ApplyAsync(Arg.Any<StockAdjustmentRequest>(), Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1, movementId: 9, newBalance: 4));

        await using var authFactory = _factory.WithWebHostBuilder(b =>
        {
            b.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "Test";
                        options.DefaultChallengeScheme = "Test";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
            });
        });

        var authClient = authFactory.CreateClient();
        var response = await authClient.PostAsJsonAsync("/api/admin/stock/adjustments", new StockAdjustmentRequest
        {
            ProductId = 1,
            StockLocationId = 1,
            QuantityChange = 1,
            Reason = "test"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
