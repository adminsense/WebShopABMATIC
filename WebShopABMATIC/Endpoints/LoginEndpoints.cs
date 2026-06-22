using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Web.Forms;

namespace WebShopABMATIC.Web.Endpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this WebApplication app)
    {
        app.MapPost("/account/admin-login", AdminLoginAsync).DisableAntiforgery();
        app.MapPost("/account/store-login", StoreLoginAsync).DisableAntiforgery();
    }

    private static async Task<IResult> AdminLoginAsync(
        HttpContext httpContext,
        ILegacySignInPort signIn,        IFormCollection form)
    {
        var login = form["login"].ToString();
        var password = form["password"].ToString();
        var rememberMe = form["rememberMe"].Count > 0;
        var returnUrl = form["returnUrl"].ToString();

        var result = await signIn.SignInStaffAsync(login, password);
        if (!result.Succeeded || result.Principal is null)
        {
            var error = Uri.EscapeDataString(result.Error ?? "Invalid login or password.");
            var safeReturn = Uri.EscapeDataString(returnUrl);
            return Results.Redirect($"/admin/login?error={error}&returnUrl={safeReturn}");
        }

        await LegacyCookieAuthentication.SignInAsync(httpContext, result.Principal, rememberMe);
        return Results.Redirect(LoginFormHelpers.ResolveAdminReturnUrl(returnUrl));
    }

    private static async Task<IResult> StoreLoginAsync(
        HttpContext httpContext,
        ILegacySignInPort signIn,
        IFormCollection form)
    {
        var login = form["login"].ToString();
        var password = form["password"].ToString();
        var rememberMe = form["rememberMe"].Count > 0;
        var returnUrl = form["returnUrl"].ToString();

        var result = await signIn.SignInCustomerAsync(login, password);
        if (!result.Succeeded || result.Principal is null)
        {
            var error = Uri.EscapeDataString(result.Error ?? "Invalid login or password.");
            var safeReturn = Uri.EscapeDataString(returnUrl);
            return Results.Redirect($"/sign-in?error={error}&returnUrl={safeReturn}");
        }

        await LegacyCookieAuthentication.SignInAsync(httpContext, result.Principal, rememberMe);
        return Results.Redirect(LoginFormHelpers.ResolveStoreReturnUrl(returnUrl));
    }
}
