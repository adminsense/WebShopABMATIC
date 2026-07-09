using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.HttpOverrides;
using WebShopABMATIC.Application;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Infrastructure;
using WebShopABMATIC.Web.Components;
using WebShopABMATIC.Web.Components.Account;
using WebShopABMATIC.Web.Endpoints;
using WebShopABMATIC.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.Configure<Microsoft.AspNetCore.SignalR.HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 2 * 1024 * 1024;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, LegacyAuthenticationStateProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/sign-in";
        options.AccessDeniedPath = "/sign-in";
        options.Cookie.Name = ".WebShopABMATIC.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/admin"))
                {
                    var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                    context.Response.Redirect($"/admin/login?returnUrl={returnUrl}");
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }

                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/admin"))
                {
                    var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                    context.Response.Redirect($"/admin/login?returnUrl={returnUrl}");
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(AppRoles.Admin))
    .AddPolicy(AppPolicies.AdminOrManager, policy => policy.RequireRole(AppRoles.Admin, AppRoles.Manager))
    .AddPolicy(AppPolicies.CustomerOnly, policy => policy.RequireRole(AppRoles.Customer));

builder.Services.AddWebShopApplication();
builder.Services.AddWebShopInfrastructure(builder.Configuration);
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 44357;
});
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedLocalStorage>();
builder.Services.AddScoped<StoreCartService>();
builder.Services.AddScoped<IGridExportService, GridExportService>();

var app = builder.Build();

app.UseForwardedHeaders();
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();
app.MapStaticAssets();
app.UseSession();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapLoginEndpoints();
app.MapStockAdjustmentApi();
app.MapStoreMediaEndpoints();
app.MapMollieWebhook();

app.MapGet("/login", () => Results.Redirect("/sign-in"));

app.MapGet("/api/store/category-icon/{id:int}", async (int id, IStoreCatalogPort catalog, CancellationToken ct) =>
{
    var bytes = await catalog.GetCategoryIconAsync(id, ct);
    if (bytes is null || bytes.Length == 0)
    {
        return Results.NotFound();
    }

    return Results.File(bytes, ImageContentType(bytes));
});

app.MapPost("/account/logout", async (HttpContext context, string? returnUrl) =>
{
    await WebShopABMATIC.Infrastructure.Auth.LegacyCookieAuthentication.SignOutAsync(context);
    return Results.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
}).DisableAntiforgery();

app.Run();

static string ImageContentType(byte[] bytes)
{
    if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
    {
        return "image/png";
    }
    if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
    {
        return "image/jpeg";
    }
    if (bytes.Length >= 6 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
    {
        return "image/gif";
    }
    if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D)
    {
        return "image/bmp";
    }
    if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0x01 && bytes[3] == 0x00)
    {
        return "image/x-icon";
    }
    return "application/octet-stream";
}
