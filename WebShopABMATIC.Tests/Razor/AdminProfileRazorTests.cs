using Moq;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Razor;

public class AdminProfileRazorTests
{
    [Fact]
    public void Profile_RendersProfileAndPasswordForms_WhenStaffProfileLoaded()
    {
        using var ctx = new BlazorFormTestContext(asAdmin: true);

        var currentUser = new Mock<ICurrentUserContext>();
        currentUser.Setup(c => c.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CurrentUserSnapshot { IsAuthenticated = true, StaffUserId = 1 });

        var staffProfile = new Mock<ILegacyStaffProfilePort>();
        staffProfile.Setup(p => p.GetAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LegacyStaffProfileDto
            {
                StaffUserId = 1,
                Login = "marco",
                Email = "marco@example.com",
                FirstName = "Marco",
                LastName = "Admin"
            });

        ctx.Services.AddSingleton(currentUser.Object);
        ctx.Services.AddSingleton(staffProfile.Object);

        var cut = ctx.RenderComponent<Profile>();

        cut.WaitForAssertion(() =>
        {
            Assert.NotNull(cut.Find("#f-first"));
            Assert.NotNull(cut.Find("#f-current"));
            Assert.Equal(2, cut.FindAll("form").Count);
        });
    }
}
