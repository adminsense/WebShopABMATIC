using Moq;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Profile;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Web.Components.Pages.Admin;
using WebShopABMATIC.Web.Components.Pages.Store;

namespace WebShopABMATIC.Tests.Razor;

public class StoreFormRazorTests
{
    [Fact]
    public void SignUp_RendersAllRequiredFields()
    {
        using var ctx = new BlazorFormTestContext();
        ctx.Services.AddSingleton(Mock.Of<ICustomerRegistrationPort>());
        ctx.Services.AddSingleton(Mock.Of<ILegacySignInPort>());

        var cut = ctx.RenderComponent<SignUp>();

        Assert.NotNull(cut.Find("#signup-email"));
        Assert.NotNull(cut.Find("#signup-password"));
        Assert.NotNull(cut.Find("#signup-confirm"));
        Assert.NotNull(cut.Find("button[type=submit]"));
    }

    [Fact]
    public void SignIn_RendersLoginForm()
    {
        using var ctx = new BlazorFormTestContext();

        var cut = ctx.RenderComponent<SignIn>();

        Assert.NotNull(cut.Find("#store-login"));
        Assert.NotNull(cut.Find("#store-password"));
        Assert.Equal("/account/store-login", cut.Find("form").GetAttribute("action"));
    }

    [Fact]
    public void AdminLogin_RendersLoginForm()
    {
        using var ctx = new BlazorFormTestContext();

        var cut = ctx.RenderComponent<AdminLogin>();

        Assert.NotNull(cut.Find("#admin-login"));
        Assert.NotNull(cut.Find("#admin-password"));
        Assert.Equal("/account/admin-login", cut.Find("form").GetAttribute("action"));
    }

    [Fact]
    public void MyAccount_RendersProfileAndPasswordForms_WhenProfileLoaded()
    {
        using var ctx = new BlazorFormTestContext(asCustomer: true);

        var profilePort = new Mock<IStoreProfilePort>();
        profilePort.Setup(p => p.GetMyProfileAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StoreProfileDto
            {
                CustomerId = 1,
                Email = "user@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                Phone = "123",
                Street = "Main",
                HouseNumber = "1",
                PostalCode = "1000",
                CityName = "Brussels"
            });
        ctx.Services.AddSingleton(profilePort.Object);
        ctx.Services.AddSingleton(Mock.Of<ICurrentUserContext>(c =>
            c.GetCurrentUserAsync(It.IsAny<CancellationToken>()) == Task.FromResult(new CurrentUserSnapshot
            {
                IsAuthenticated = true,
                CustomerId = 1
            })));
        ctx.Services.AddSingleton(Mock.Of<ILegacyCustomerPasswordPort>());

        var cut = ctx.RenderComponent<MyAccount>();

        cut.WaitForAssertion(() =>
        {
            Assert.NotNull(cut.Find("#acc-first"));
            Assert.NotNull(cut.Find("#acc-current"));
            Assert.Equal(2, cut.FindAll("form").Count);
        });
    }
}
