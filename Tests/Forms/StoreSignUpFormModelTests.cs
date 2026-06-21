using WebShopABMATIC.Application.Validation;
using WebShopABMATIC.Web.Forms;

namespace WebShopABMATIC.Tests.Forms;

public class StoreSignUpFormModelTests
{
    private static StoreSignUpFormModel ValidModel() => new()
    {
        Email = "user@example.com",
        FirstName = "Jane",
        LastName = "Doe",
        Phone = "+32 470 12 34 56",
        Street = "Main Street",
        HouseNumber = "10",
        PostalCode = "1000",
        CityName = "Brussels",
        Password = "Secret1!",
        ConfirmPassword = "Secret1!"
    };

    [Fact]
    public void ValidModel_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(ValidModel());
    }

    [Theory]
    [InlineData(nameof(StoreSignUpFormModel.Email))]
    [InlineData(nameof(StoreSignUpFormModel.FirstName))]
    [InlineData(nameof(StoreSignUpFormModel.LastName))]
    [InlineData(nameof(StoreSignUpFormModel.Phone))]
    [InlineData(nameof(StoreSignUpFormModel.Street))]
    [InlineData(nameof(StoreSignUpFormModel.HouseNumber))]
    [InlineData(nameof(StoreSignUpFormModel.PostalCode))]
    [InlineData(nameof(StoreSignUpFormModel.CityName))]
    [InlineData(nameof(StoreSignUpFormModel.Password))]
    [InlineData(nameof(StoreSignUpFormModel.ConfirmPassword))]
    public void RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = ValidModel();
        typeof(StoreSignUpFormModel).GetProperty(propertyName)!.SetValue(model, "");
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }

    [Fact]
    public void Password_WhenMissingUppercase_IsInvalid()
    {
        var model = ValidModel();
        model.Password = "secret1!";
        model.ConfirmPassword = "secret1!";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreSignUpFormModel.Password));
    }

    [Fact]
    public void Password_WhenMissingLowercase_IsInvalid()
    {
        var model = ValidModel();
        model.Password = "SECRET1!";
        model.ConfirmPassword = "SECRET1!";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreSignUpFormModel.Password));
    }

    [Fact]
    public void Password_WhenMissingDigit_IsInvalid()
    {
        var model = ValidModel();
        model.Password = "Secret!!";
        model.ConfirmPassword = "Secret!!";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreSignUpFormModel.Password));
    }

    [Fact]
    public void ConfirmPassword_WhenMismatch_IsInvalid()
    {
        var model = ValidModel();
        model.ConfirmPassword = "Other1!";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreSignUpFormModel.ConfirmPassword));
    }

    [Fact]
    public void Email_WhenInvalidFormat_IsInvalid()
    {
        var model = ValidModel();
        model.Email = "not-an-email";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreSignUpFormModel.Email));
    }

    [Fact]
    public void Password_WhenShorterThanEight_IsInvalid()
    {
        var model = ValidModel();
        model.Password = "Sec1!";
        model.ConfirmPassword = "Sec1!";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreSignUpFormModel.Password));
    }
}
