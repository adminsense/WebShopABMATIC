using WebShopABMATIC.Application.Store.Profile;
using WebShopABMATIC.Web.Forms;

namespace WebShopABMATIC.Tests.Forms;

public class StoreAccountFormModelsTests
{
    private static StoreAccountProfileFormModel ValidProfile() => new()
    {
        FirstName = "Jane",
        LastName = "Doe",
        Phone = "+32 470 12 34 56",
        Street = "Main Street",
        HouseNumber = "10",
        PostalCode = "1000",
        CityName = "Brussels"
    };

    private static StoreAccountPasswordFormModel ValidPassword() => new()
    {
        CurrentPassword = "OldPass1!",
        NewPassword = "NewPass1!",
        ConfirmPassword = "NewPass1!"
    };

    [Fact]
    public void Profile_ValidModel_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(ValidProfile());
    }

    [Theory]
    [InlineData(nameof(StoreAccountProfileFormModel.FirstName))]
    [InlineData(nameof(StoreAccountProfileFormModel.LastName))]
    [InlineData(nameof(StoreAccountProfileFormModel.Phone))]
    [InlineData(nameof(StoreAccountProfileFormModel.Street))]
    [InlineData(nameof(StoreAccountProfileFormModel.HouseNumber))]
    [InlineData(nameof(StoreAccountProfileFormModel.PostalCode))]
    [InlineData(nameof(StoreAccountProfileFormModel.CityName))]
    public void Profile_RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = ValidProfile();
        typeof(StoreAccountProfileFormModel).GetProperty(propertyName)!.SetValue(model, "");
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }

    [Fact]
    public void Profile_FromDto_MapsAllFields()
    {
        var dto = new StoreProfileDto
        {
            FirstName = "A",
            LastName = "B",
            Phone = "123",
            Street = "S",
            HouseNumber = "1",
            Box = "  ",
            PostalCode = "9000",
            CityName = "Gent"
        };

        var model = StoreAccountProfileFormModel.FromDto(dto);
        Assert.Null(model.Box);
        Assert.Equal("Gent", model.CityName);
    }

    [Fact]
    public void Profile_ToUpdateDto_MapsAllFields()
    {
        var model = ValidProfile();
        model.Box = "B";
        var dto = model.ToUpdateDto();
        Assert.Equal("Jane", dto.FirstName);
        Assert.Equal("B", dto.Box);
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
        model.ConfirmPassword = "Different1!";
        FormValidationTestHelper.AssertInvalid(model, nameof(StoreAccountPasswordFormModel.ConfirmPassword));
    }

    [Theory]
    [InlineData(nameof(StoreAccountPasswordFormModel.CurrentPassword))]
    [InlineData(nameof(StoreAccountPasswordFormModel.NewPassword))]
    [InlineData(nameof(StoreAccountPasswordFormModel.ConfirmPassword))]
    public void Password_RequiredField_WhenEmpty_IsInvalid(string propertyName)
    {
        var model = ValidPassword();
        typeof(StoreAccountPasswordFormModel).GetProperty(propertyName)!.SetValue(model, "");
        FormValidationTestHelper.AssertInvalid(model, propertyName);
    }
}
