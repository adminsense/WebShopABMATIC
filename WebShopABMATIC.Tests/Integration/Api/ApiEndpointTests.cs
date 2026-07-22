using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Tests.Integration.Api;

public sealed class WebShopApiFactory : WebApplicationFactory<Program>
{
    public ILegacySignInPort SignIn { get; } = Substitute.For<ILegacySignInPort>();
    public IMollieWebhookPort MollieWebhook { get; } = Substitute.For<IMollieWebhookPort>();
    public IStockAdjustmentPort StockAdjustment { get; } = Substitute.For<IStockAdjustmentPort>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:connWebShopABMATIC"] =
                    "Server=(localdb)\\mssqllocaldb;Database=WebShopABMATIC_ApiTests_Unused;Trusted_Connection=True;TrustServerCertificate=True",
                ["Mollie:UseMock"] = "true",
                ["AzureBlob:ConnectionString"] = "",
                ["AzureBlob:ContainerName"] = "test"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            foreach (var hosted in services.Where(d => d.ServiceType == typeof(IHostedService)).ToList())
            {
                services.Remove(hosted);
            }

            services.RemoveAll<DbContextOptions<WebShopABMATICDbContext>>();
            services.RemoveAll<WebShopABMATICDbContext>();
            services.AddDbContext<WebShopABMATICDbContext>(options =>
                options.UseInMemoryDatabase("api-tests-" + Guid.NewGuid().ToString("N")));

            services.RemoveAll<ILegacySignInPort>();
            services.AddSingleton(SignIn);

            services.RemoveAll<IMollieWebhookPort>();
            services.AddSingleton(MollieWebhook);

            services.RemoveAll<IStockAdjustmentPort>();
            services.AddSingleton(StockAdjustment);
        });
    }
}

public sealed class LoginEndpointsTests : IClassFixture<WebShopApiFactory>
{
    private readonly WebShopApiFactory _factory;

    public LoginEndpointsTests(WebShopApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Store_login_redirects_home_on_success()
    {
        var principal = CreateCustomerPrincipal();
        _factory.SignIn.SignInCustomerAsync("user", "pass", Arg.Any<CancellationToken>())
            .Returns(new LegacySignInResult { Succeeded = true, Principal = principal });

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.PostAsync("/account/store-login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["login"] = "user",
            ["password"] = "pass",
            ["returnUrl"] = "/"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Be("/");
    }

    [Fact]
    public async Task Store_login_redirects_to_sign_in_on_failure()
    {
        _factory.SignIn.SignInCustomerAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new LegacySignInResult { Succeeded = false, Error = "Invalid login or password." });

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.PostAsync("/account/store-login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["login"] = "bad",
            ["password"] = "bad"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().StartWith("/sign-in?");
    }

    [Fact]
    public async Task Admin_login_redirects_on_success()
    {
        var principal = CreateStaffPrincipal();
        _factory.SignIn.SignInStaffAsync("admin", "secret", Arg.Any<CancellationToken>())
            .Returns(new LegacySignInResult { Succeeded = true, Principal = principal });

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.PostAsync("/account/admin-login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["login"] = "admin",
            ["password"] = "secret"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.ToString().Should().Contain("/admin");
    }

    private static ClaimsPrincipal CreateCustomerPrincipal()
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, "user"),
            new Claim(ClaimTypes.Role, AppRoles.Customer),
            new Claim(LegacyAuthClaims.CustomerId, "1")
        ], "Legacy");
        return new ClaimsPrincipal(identity);
    }

    private static ClaimsPrincipal CreateStaffPrincipal()
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, AppRoles.Admin),
            new Claim(LegacyAuthClaims.StaffUserId, "1")
        ], "Legacy");
        return new ClaimsPrincipal(identity);
    }
}

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

public sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        Microsoft.Extensions.Options.IOptionsMonitor<AuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, "manager"),
            new Claim(ClaimTypes.Role, AppRoles.Manager),
            new Claim(LegacyAuthClaims.StaffUserId, "1")
        ], "Test");
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), "Test");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
