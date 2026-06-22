using WebShopABMATIC.Web.Forms;

namespace WebShopABMATIC.Tests.Forms;

public class IdentityAccountFormModelTests
{
    [Theory]
    [InlineData(typeof(IdentityRegisterFormModel), nameof(IdentityRegisterFormModel.Email))]
    [InlineData(typeof(IdentityForgotPasswordFormModel), nameof(IdentityForgotPasswordFormModel.Email))]
    [InlineData(typeof(IdentityResetPasswordFormModel), nameof(IdentityResetPasswordFormModel.Code))]
    [InlineData(typeof(IdentityResendEmailConfirmationFormModel), nameof(IdentityResendEmailConfirmationFormModel.Email))]
    [InlineData(typeof(IdentityExternalLoginConfirmationFormModel), nameof(IdentityExternalLoginConfirmationFormModel.Email))]
    [InlineData(typeof(IdentityLoginWith2FaFormModel), nameof(IdentityLoginWith2FaFormModel.TwoFactorCode))]
    [InlineData(typeof(IdentityLoginWithRecoveryCodeFormModel), nameof(IdentityLoginWithRecoveryCodeFormModel.RecoveryCode))]
    [InlineData(typeof(IdentityManageChangePasswordFormModel), nameof(IdentityManageChangePasswordFormModel.OldPassword))]
    [InlineData(typeof(IdentityManageSetPasswordFormModel), nameof(IdentityManageSetPasswordFormModel.NewPassword))]
    [InlineData(typeof(IdentityManageEmailFormModel), nameof(IdentityManageEmailFormModel.NewEmail))]
    [InlineData(typeof(IdentityManageDeletePersonalDataFormModel), nameof(IdentityManageDeletePersonalDataFormModel.Password))]
    [InlineData(typeof(IdentityManageEnableAuthenticatorFormModel), nameof(IdentityManageEnableAuthenticatorFormModel.Code))]
    public void RequiredField_WhenEmpty_IsInvalid(Type modelType, string propertyName)
    {
        var model = Activator.CreateInstance(modelType)!;
        modelType.GetProperty(propertyName)!.SetValue(model, modelType.GetProperty(propertyName)!.PropertyType == typeof(string) ? "" : null);
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }

    [Fact]
    public void IdentityRegister_ValidModel_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(new IdentityRegisterFormModel
        {
            Email = "user@example.com",
            Password = "Passw0rd",
            ConfirmPassword = "Passw0rd"
        });
    }

    [Fact]
    public void IdentityRegister_ConfirmPasswordMismatch_IsInvalid()
    {
        var model = new IdentityRegisterFormModel
        {
            Email = "user@example.com",
            Password = "Passw0rd",
            ConfirmPassword = "Other12"
        };
        FormValidationTestHelper.AssertInvalid(model, nameof(IdentityRegisterFormModel.ConfirmPassword));
    }

    [Fact]
    public void IdentityManageProfile_Phone_IsOptional()
    {
        FormValidationTestHelper.AssertValid(new IdentityManageProfileFormModel());
    }

    [Fact]
    public void IdentityManageChangePassword_ValidModel_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(new IdentityManageChangePasswordFormModel
        {
            OldPassword = "OldPass1",
            NewPassword = "NewPass1",
            ConfirmPassword = "NewPass1"
        });
    }
}
