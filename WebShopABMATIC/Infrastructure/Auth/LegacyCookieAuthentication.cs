using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Infrastructure.Auth;

public static class LegacyCookieAuthentication
{
    public const string Scheme = CookieAuthenticationDefaults.AuthenticationScheme;
    public const string CookieName = ".WebShopABMATIC.Auth.Session";
    private const string LegacyCookieName = ".WebShopABMATIC.Auth";

    /// <summary>Idle session length aligned with cookie ExpireTimeSpan.</summary>
    public static readonly TimeSpan SessionIdleTimeout = TimeSpan.FromMinutes(15);

    public static Task SignInAsync(HttpContext httpContext, ClaimsPrincipal principal, bool isPersistent)
    {
        var properties = new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            AllowRefresh = true,
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.Add(SessionIdleTimeout)
        };

        return httpContext.SignInAsync(Scheme, principal, properties);
    }

    public static async Task SignOutAsync(HttpContext httpContext)
    {
        var sessionId = httpContext.User.FindFirstValue(LegacyAuthClaims.StoreBrowserSession);
        if (!string.IsNullOrEmpty(sessionId))
        {
            httpContext.RequestServices.GetRequiredService<IStoreBrowserSessionStore>().End(sessionId);
        }

        await httpContext.SignOutAsync(Scheme);

        var cookieOptions = new CookieOptions
        {
            Path = "/",
            HttpOnly = true,
            Secure = httpContext.Request.IsHttps,
            SameSite = SameSiteMode.Lax
        };

        httpContext.Response.Cookies.Delete(CookieName, cookieOptions);
        httpContext.Response.Cookies.Delete(LegacyCookieName, cookieOptions);
    }

    public static ClaimsPrincipal AttachStoreBrowserSession(ClaimsPrincipal principal, IStoreBrowserSessionStore sessions)
    {
        if (principal.Identity is not ClaimsIdentity identity
            || !principal.IsInRole(AppRoles.Customer))
        {
            return principal;
        }

        if (identity.FindFirst(LegacyAuthClaims.StoreBrowserSession) is not null)
        {
            return principal;
        }

        identity.AddClaim(new Claim(LegacyAuthClaims.StoreBrowserSession, sessions.Start()));
        return principal;
    }
}
