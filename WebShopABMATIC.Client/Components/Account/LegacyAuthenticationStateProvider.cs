using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Web.Components.Account;

/// <summary>
/// Cookie-only auth for Blazor circuits. Never revives identity from prerender persisted state —
/// that caused "still logged in" after Sign out when HttpContext was unavailable on the circuit.
/// </summary>
public sealed class LegacyAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LegacyAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor)
        : base(loggerFactory)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(30);

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpUser = _httpContextAccessor.HttpContext?.User;
        if (httpUser?.Identity?.IsAuthenticated == true)
        {
            return Task.FromResult(new AuthenticationState(httpUser));
        }

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    }

    protected override Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult(false);
        }

        var user = authenticationState.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            return Task.FromResult(false);
        }

        var hasLegacyId = user.HasClaim(c =>
            c.Type is ClaimTypes.NameIdentifier or "legacy_staff_id" or "legacy_customer_id");

        return Task.FromResult(hasLegacyId);
    }
}
