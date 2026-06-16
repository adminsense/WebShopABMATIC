using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace WebShopABMATIC.Infrastructure.Auth;

public static class LegacyCookieAuthentication
{
    public const string Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

    public static Task SignInAsync(HttpContext httpContext, ClaimsPrincipal principal, bool isPersistent) =>
        httpContext.SignInAsync(Scheme, principal, new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            AllowRefresh = true
        });

    public static Task SignOutAsync(HttpContext httpContext) =>
        httpContext.SignOutAsync(Scheme);
}
