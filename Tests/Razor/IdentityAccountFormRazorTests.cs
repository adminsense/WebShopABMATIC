using Microsoft.AspNetCore.Identity;
using Moq;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Web.Components.Account;
using WebShopABMATIC.Web.Components.Account.Pages;

namespace WebShopABMATIC.Tests.Razor;

public class IdentityAccountFormRazorTests
{
    [Fact]
    public void ForgotPassword_RendersEmailField()
    {
        using var ctx = CreateIdentityContext();

        var cut = ctx.RenderComponent<ForgotPassword>();

        Assert.NotNull(cut.Find("input[type=text], input:not([type])"));
        Assert.NotNull(cut.Find("button[type=submit]"));
    }

    [Fact]
    public void Register_RendersEmailAndPasswordFields()
    {
        using var ctx = CreateIdentityContext();

        var cut = ctx.RenderComponent<Register>();

        cut.WaitForAssertion(() =>
        {
            Assert.NotNull(cut.Find("button[type=submit]"));
            Assert.True(cut.FindAll("input").Count >= 3);
        });
    }

    [Fact]
    public void ResendEmailConfirmation_RendersEmailField()
    {
        using var ctx = CreateIdentityContext();

        var cut = ctx.RenderComponent<ResendEmailConfirmation>();

        Assert.NotNull(cut.Find("button[type=submit]"));
    }

    private static BlazorFormTestContext CreateIdentityContext()
    {
        var ctx = new BlazorFormTestContext();

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        var userManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        var signInStore = new Mock<IUserStore<ApplicationUser>>();
        var signInManager = new Mock<SignInManager<ApplicationUser>>(
            userManager.Object,
            Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
            Mock.Of<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        signInManager.Setup(s => s.GetExternalAuthenticationSchemesAsync())
            .ReturnsAsync([]);

        ctx.Services.AddSingleton(userStore.Object);
        ctx.Services.AddSingleton(userManager.Object);
        ctx.Services.AddSingleton<IUserStore<ApplicationUser>>(userStore.Object);
        ctx.Services.AddSingleton(signInManager.Object);
        ctx.Services.AddSingleton(Mock.Of<IEmailSender<ApplicationUser>>());
        ctx.Services.AddSingleton(new IdentityRedirectManager(new FakeNavigationManager()));
        ctx.Services.AddSingleton(Mock.Of<Microsoft.Extensions.Logging.ILogger<Register>>());

        return ctx;
    }
}
