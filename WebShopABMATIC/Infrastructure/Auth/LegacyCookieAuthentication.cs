using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace WebShopABMATIC.Infrastructure.Auth;

public static class LegacyCookieAuthentication
{
    public const string Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

    /// <summary>Current auth cookie. Bump suffix when invalidating stuck client cookies.</summary>
    public const string CookieName = ".WebShopABMATIC.Auth.Session.v2";

    private static readonly string[] LegacyCookieNames =
    [
        ".WebShopABMATIC.Auth",
        ".WebShopABMATIC.Auth.Session"
    ];

    /// <summary>Idle ticket lifetime (sliding) inside the auth cookie.</summary>
    public static readonly TimeSpan SessionIdleTimeout = TimeSpan.FromMinutes(15);

    public static async Task SignInAsync(HttpContext httpContext, ClaimsPrincipal principal, bool isPersistent)
    {
        // Always clear prior cookies first (incl. old names / sticky sessions).
        DeleteAuthCookies(httpContext);

        // Never persist across browser restarts. Chrome restores "session" cookies via
        // "Continue where you left off"; we use MaxAge on the cookie options instead.
        _ = isPersistent;
        var issued = DateTimeOffset.UtcNow;
        var properties = new AuthenticationProperties
        {
            IsPersistent = false,
            AllowRefresh = true,
            IssuedUtc = issued,
            ExpiresUtc = issued.Add(SessionIdleTimeout)
        };

        await httpContext.SignInAsync(Scheme, principal, properties);
    }

    public static async Task SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(Scheme);
        DeleteAuthCookies(httpContext);

        httpContext.Response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
        httpContext.Response.Headers.Pragma = "no-cache";
        httpContext.Response.Headers.Expires = "0";
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

        void Wipe(string name)
        {
            httpContext.Response.Cookies.Delete(name, cookieOptions);
            httpContext.Response.Cookies.Append(name, string.Empty, cookieOptions);
        }

        Wipe(CookieName);
        foreach (var name in LegacyCookieNames)
        {
            Wipe(name);
        }
    }
}
