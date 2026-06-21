using WebShopABMATIC.Application.Validation;

namespace WebShopABMATIC.Tests.Forms;

public class FormValidationHelperTests
{
    private sealed class SampleModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; } = "";
    }

    [Fact]
    public void Validate_ReturnsEmpty_WhenModelIsValid()
    {
        var model = new SampleModel { Name = "ok" };
        Assert.Empty(FormValidationHelper.Validate(model));
    }

    [Fact]
    public void Validate_ReturnsErrors_WhenModelIsInvalid()
    {
        var model = new SampleModel();
        var results = FormValidationHelper.Validate(model);
        Assert.NotEmpty(results);
        Assert.True(FormValidationHelper.HasErrorFor(model, nameof(SampleModel.Name)));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenInvalid()
    {
        Assert.False(FormValidationHelper.IsValid(new SampleModel()));
    }
}
