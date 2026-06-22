using WebShopABMATIC.Web.Forms;

namespace WebShopABMATIC.Tests.Forms;

public class AdminProfileFormModelsTests
{
    private static AdminProfileFormModel ValidProfile() => new()
    {
        FirstName = "Marco",
        LastName = "Admin",
        PhoneNumber = "+32 470 00 00 00"
    };

    private static AdminPasswordFormModel ValidPassword() => new()
    {
        CurrentPassword = "Current1",
        NewPassword = "NewPass1",
        ConfirmPassword = "NewPass1"
    };

    [Fact]
    public void Profile_ValidModel_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(ValidProfile());
    }

    [Theory]
    [InlineData(nameof(AdminProfileFormModel.FirstName))]
    [InlineData(nameof(AdminProfileFormModel.LastName))]
    public void Profile_RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = ValidProfile();
        typeof(AdminProfileFormModel).GetProperty(propertyName)!.SetValue(model, null);
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }

    [Fact]
    public void Profile_Phone_IsOptional()
    {
        var model = ValidProfile();
        model.PhoneNumber = null;
        FormValidationTestHelper.AssertValid(model);
    }

    [Fact]
    public void Password_ValidModel_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(ValidPassword());
    }

    [Fact]
    public void Password_WhenConfirmMismatch_IsInvalid()
    {
        var model = ValidPassword();
        model.ConfirmPassword = "Other";
        FormValidationTestHelper.AssertInvalid(model, nameof(AdminPasswordFormModel.ConfirmPassword));
    }

    [Theory]
    [InlineData(nameof(AdminPasswordFormModel.CurrentPassword))]
    [InlineData(nameof(AdminPasswordFormModel.NewPassword))]
    [InlineData(nameof(AdminPasswordFormModel.ConfirmPassword))]
    public void Password_RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = ValidPassword();
        typeof(AdminPasswordFormModel).GetProperty(propertyName)!.SetValue(model, "");
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }
}
