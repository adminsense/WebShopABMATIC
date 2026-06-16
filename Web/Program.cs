using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebShopABMATIC.Application;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Infrastructure;
using WebShopABMATIC.Web.Components;
using WebShopABMATIC.Web.Components.Account;
using WebShopABMATIC.Web.Endpoints;
using WebShopABMATIC.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = builder.Environment.IsDevelopment();
    });

builder.Services.Configure<Microsoft.AspNetCore.SignalR.HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 2 * 1024 * 1024;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, LegacyAuthenticationStateProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/sign-in";
        options.AccessDeniedPath = "/sign-in";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                var loginPath = context.Request.Path.StartsWithSegments("/admin")
                    ? $"/admin/login?returnUrl={returnUrl}"
                    : $"/sign-in?returnUrl={returnUrl}";
                context.Response.Redirect(loginPath);
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                var loginPath = context.Request.Path.StartsWithSegments("/admin")
                    ? $"/admin/login?returnUrl={returnUrl}"
                    : $"/sign-in?returnUrl={returnUrl}";
                context.Response.Redirect(loginPath);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(AppRoles.Admin))
    .AddPolicy(AppPolicies.AdminOrManager, policy => policy.RequireRole(AppRoles.Admin, AppRoles.Manager))
    .AddPolicy(AppPolicies.CustomerOnly, policy => policy.RequireRole(AppRoles.Customer));

builder.Services.AddWebShopApplication();
builder.Services.AddWebShopInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddScoped<StoreCartService>();
builder.Services.AddScoped<IGridExportService, GridExportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapStockAdjustmentApi();

app.MapPost("/account/logout", async (HttpContext context, string? returnUrl) =>
{
    await WebShopABMATIC.Infrastructure.Auth.LegacyCookieAuthentication.SignOutAsync(context);
    return Results.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
}).DisableAntiforgery();

app.Run();
