using System.Net;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Tests.Integration.Api;

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
