using Bunit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Tests.Razor;

internal sealed class BlazorFormTestContext : TestContext
{
    public BlazorFormTestContext(bool asAdmin = false, bool asCustomer = false)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddLogging();
        Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly, p => p.RequireRole(AppRoles.Admin));
            options.AddPolicy(AppPolicies.AdminOrManager, p => p.RequireRole(AppRoles.Admin, AppRoles.Manager));
            options.AddPolicy(AppPolicies.CustomerOnly, p => p.RequireRole(AppRoles.Customer));
        });
        Services.AddSingleton<AuthenticationStateProvider>(new TestAuthStateProvider(asAdmin, asCustomer));
        Services.AddSingleton<NavigationManager, FakeNavigationManager>();
    }
}

internal sealed class FakeNavigationManager : NavigationManager
{
    public FakeNavigationManager()
    {
        Initialize("http://localhost/", "http://localhost/");
    }

    protected override void NavigateToCore(string uri, bool forceLoad) =>
        Uri = ToAbsoluteUri(uri).ToString();
}

internal static class ComponentRenderExtensions
{
    public static IRenderedFragment RenderFormComponent(this BlazorFormTestContext ctx, Type componentType)
    {
        var renderMethod = typeof(TestContext)
            .GetMethods()
            .First(m => m.Name == nameof(TestContext.RenderComponent)
                        && m.IsGenericMethod
                        && m.GetParameters().Length == 1
                        && m.GetParameters()[0].ParameterType == typeof(ComponentParameter[]));

        var generic = renderMethod.MakeGenericMethod(componentType);
        return (IRenderedFragment)generic.Invoke(ctx, new object[] { Array.Empty<ComponentParameter>() })!;
    }
}
