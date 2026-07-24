using System.Security.Claims;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Tests.TestDoubles;
using WebShopABMATIC.Web.Services;

namespace WebShopABMATIC.Tests.Bunit;

public abstract class BunitStoreTestBase : TestContext
{
    protected IStoreCatalogPort Catalog { get; } = Substitute.For<IStoreCatalogPort>();
    protected ICheckoutPort Checkout { get; } = Substitute.For<ICheckoutPort>();
    protected IProductAdminPort ProductAdmin { get; } = Substitute.For<IProductAdminPort>();
    protected IAdminDashboardPort Dashboard { get; } = Substitute.For<IAdminDashboardPort>();
    protected ICurrentUserContext CurrentUser { get; } = Substitute.For<ICurrentUserContext>();
    protected TestAuthorizationContext Auth { get; }

    protected BunitStoreTestBase()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        Services.AddSingleton(Catalog);
        Services.AddSingleton(Checkout);
        Services.AddSingleton(ProductAdmin);
        Services.AddSingleton(Dashboard);
        Services.AddSingleton(CurrentUser);
        Services.AddSingleton(Substitute.For<ILegacySignInPort>());
        Services.AddLogging();

        Services.AddSingleton<IStoreCartSessionStore, InMemoryStoreCartSessionStore>();
        // Singleton in tests so pre-seed and render share the same cart instance.
        Services.AddSingleton<StoreCartService>();
        Services.AddSingleton(Substitute.For<IGridExportService>());

        Auth = this.AddTestAuthorization();
        AsGuest();
    }

    protected void AsGuest()
    {
        Auth.SetNotAuthorized();
        CurrentUser.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(CurrentUserSnapshot.Anonymous);
    }

    protected void AsCustomer(int customerId = 10, string email = "buyer@test.local")
    {
        Auth.SetAuthorized(email);
        Auth.SetRoles(AppRoles.Customer);
        Auth.SetPolicies(AppPolicies.CustomerOnly);
        Auth.SetClaims(
            new Claim(LegacyAuthClaims.CustomerId, customerId.ToString()),
            new Claim(LegacyAuthClaims.Login, email),
            new Claim(LegacyAuthClaims.DisplayName, email));
        CurrentUser.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot
            {
                IsAuthenticated = true,
                CustomerId = customerId,
                DisplayName = email,
                AuditLabel = email
            });
    }

    protected void AsStaff(int staffUserId = 1, string email = "staff@test.local")
    {
        Auth.SetAuthorized(email);
        Auth.SetRoles(AppRoles.Admin);
        Auth.SetPolicies(AppPolicies.AdminOrManager, AppPolicies.AdminOnly);
        Auth.SetClaims(
            new Claim(LegacyAuthClaims.StaffUserId, staffUserId.ToString()),
            new Claim(LegacyAuthClaims.Login, email),
            new Claim(LegacyAuthClaims.DisplayName, email));
        CurrentUser.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot
            {
                IsAuthenticated = true,
                StaffUserId = staffUserId,
                DisplayName = email,
                AuditLabel = email
            });
    }
}
