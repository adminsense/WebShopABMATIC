using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WebShopABMATIC.Application;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Infrastructure;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Web.Components;
using WebShopABMATIC.Web.Components.Account;
using WebShopABMATIC.Web.Endpoints;
using WebShopABMATIC.Infrastructure.Seeding;
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
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppPolicies.AdminOnly, policy => policy.RequireRole(AppRoles.Admin))
    .AddPolicy(AppPolicies.AdminOrManager, policy => policy.RequireRole(AppRoles.Admin, AppRoles.Manager))
    .AddPolicy(AppPolicies.CustomerOnly, policy => policy.RequireRole(AppRoles.Customer));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddWebShopApplication();
builder.Services.AddWebShopInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddScoped<StoreCartService>();
builder.Services.AddScoped<IGridExportService, GridExportService>();
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Server.Circuits.CircuitHandler, WebShopABMATIC.Infrastructure.Audit.AuditCircuitHandler>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

if (args.Contains("--seed-identity", StringComparer.OrdinalIgnoreCase))
{
    await IdentitySeed.SeedAsync(app.Services);
    return;
}

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

app.MapMollieWebhook();
app.MapStockAdjustmentApi();
app.MapAdditionalIdentityEndpoints();

app.Run();
