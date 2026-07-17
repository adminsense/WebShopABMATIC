using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
}
