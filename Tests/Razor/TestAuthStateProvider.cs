using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Tests.Razor;

internal sealed class TestAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _state;

    public TestAuthStateProvider(bool asAdmin = false, bool asCustomer = false)
    {
        var claims = new List<Claim>();
        if (asAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, AppRoles.Admin));
            claims.Add(new Claim(LegacyAuthClaims.StaffUserId, "1"));
        }

        if (asCustomer)
        {
            claims.Add(new Claim(ClaimTypes.Role, AppRoles.Customer));
            claims.Add(new Claim(LegacyAuthClaims.CustomerId, "1"));
        }

        var identity = new ClaimsIdentity(claims, authenticationType: "test");
        _state = new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(_state);
}
