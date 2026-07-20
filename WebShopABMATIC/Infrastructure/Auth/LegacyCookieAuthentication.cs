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
        // Store customers: IsPersistent=false → browser session cookie (cleared when browser closes).
        // Do not set ExpiresUtc on the cookie properties when non-persistent — that can make the
        // cookie survive Sign out / browser restart incorrectly.
        var properties = new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            AllowRefresh = true,
            IssuedUtc = DateTimeOffset.UtcNow
        };

        if (isPersistent)
        {
            properties.ExpiresUtc = DateTimeOffset.UtcNow.Add(SessionIdleTimeout);
        }

        return httpContext.SignInAsync(Scheme, principal, properties);
    }

    public static async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(Scheme);

        // Delete must match cookie options used at sign-in (path / SameSite / Secure).
        var cookieOptions = new CookieOptions
        {
            Path = "/",
            HttpOnly = true,
            Secure = httpContext.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Expires = DateTimeOffset.UnixEpoch,
            MaxAge = TimeSpan.Zero
        };

        httpContext.Response.Cookies.Delete(CookieName, cookieOptions);
        httpContext.Response.Cookies.Delete(LegacyCookieName, cookieOptions);
        httpContext.Response.Cookies.Append(CookieName, string.Empty, cookieOptions);
        httpContext.Response.Cookies.Append(LegacyCookieName, string.Empty, cookieOptions);

        httpContext.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
        httpContext.Response.Headers.Pragma = "no-cache";
    }
}
