using WebShopABMATIC.Web.Forms;

namespace WebShopABMATIC.Tests.Forms;

public class LoginFormTests
{
    [Theory]
    [InlineData(null, "/admin")]
    [InlineData("", "/admin")]
    [InlineData("   ", "/admin")]
    [InlineData("/admin/orders", "/admin/orders")]
    [InlineData("//evil.com", "/admin")]
    [InlineData("https://evil.com", "/admin")]
    public void ResolveAdminReturnUrl_ReturnsSafeLocalOrDefault(string? returnUrl, string expected)
    {
        Assert.Equal(expected, LoginFormHelpers.ResolveAdminReturnUrl(returnUrl));
    }

    [Theory]
    [InlineData(null, "/")]
    [InlineData("", "/")]
    [InlineData("   ", "/")]
    [InlineData("/checkout", "/checkout")]
    [InlineData("//evil.com", "/")]
    [InlineData("https://evil.com", "/")]
    public void ResolveStoreReturnUrl_ReturnsSafeLocalOrDefault(string? returnUrl, string expected)
    {
        Assert.Equal(expected, LoginFormHelpers.ResolveStoreReturnUrl(returnUrl));
    }

    [Fact]
    public void AdminLoginForm_ValidModel_PassesValidation()
    {
        var model = new AdminLoginFormModel { Login = "marco", Password = "secret" };
        FormValidationTestHelper.AssertValid(model);
    }

    [Theory]
    [InlineData(nameof(AdminLoginFormModel.Login))]
    [InlineData(nameof(AdminLoginFormModel.Password))]
    public void AdminLoginForm_RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = new AdminLoginFormModel { Login = "marco", Password = "secret" };
        typeof(AdminLoginFormModel).GetProperty(propertyName)!.SetValue(model, "");
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }

    [Fact]
    public void StoreLoginForm_ValidModel_PassesValidation()
    {
        var model = new StoreLoginFormModel { Login = "customer@example.com", Password = "secret" };
        FormValidationTestHelper.AssertValid(model);
    }

    [Theory]
    [InlineData(nameof(StoreLoginFormModel.Login))]
    [InlineData(nameof(StoreLoginFormModel.Password))]
    public void StoreLoginForm_RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = new StoreLoginFormModel { Login = "customer@example.com", Password = "secret" };
        typeof(StoreLoginFormModel).GetProperty(propertyName)!.SetValue(model, "");
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }
}
