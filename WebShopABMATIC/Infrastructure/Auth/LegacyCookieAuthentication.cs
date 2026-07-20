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

    /// <summary>Idle ticket lifetime (sliding) inside the auth cookie.</summary>
    public static readonly TimeSpan SessionIdleTimeout = TimeSpan.FromMinutes(15);

    public static async Task SignInAsync(HttpContext httpContext, ClaimsPrincipal principal, bool isPersistent)
    {
        // Drop any legacy persistent cookie so store customers never inherit an old session.
        DeleteAuthCookies(httpContext);

        var properties = new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            AllowRefresh = true,
            IssuedUtc = DateTimeOffset.UtcNow
        };

        // Only staff "remember me" gets an explicit expiry. Store customers = session cookie (no Expires).
        if (isPersistent)
        {
            properties.ExpiresUtc = DateTimeOffset.UtcNow.Add(SessionIdleTimeout);
        }

        await httpContext.SignInAsync(Scheme, principal, properties);
    }

    public static async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(Scheme);
        DeleteAuthCookies(httpContext);

        httpContext.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
        httpContext.Response.Headers.Pragma = "no-cache";
    }

    private static void DeleteAuthCookies(HttpContext httpContext)
    {
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
    }
}
