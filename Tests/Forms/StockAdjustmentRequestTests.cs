using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Tests.Forms;

public class StockAdjustmentRequestTests
{
    private static StockAdjustmentRequest ValidRequest() => new()
    {
        ProductId = 100,
        StockLocationId = 1,
        QuantityChange = -5m,
        Reason = "Cycle count correction"
    };

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(ValidRequest());
    }

    [Fact]
    public void ProductId_WhenZero_IsInvalid()
    {
        var model = ValidRequest();
        model.ProductId = 0;
        FormValidationTestHelper.AssertInvalid(model, nameof(StockAdjustmentRequest.ProductId));
    }

    [Fact]
    public void StockLocationId_WhenZero_IsInvalid()
    {
        var model = ValidRequest();
        model.StockLocationId = 0;
        FormValidationTestHelper.AssertInvalid(model, nameof(StockAdjustmentRequest.StockLocationId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void Reason_WhenTooShort_IsInvalid(string reason)
    {
        var model = ValidRequest();
        model.Reason = reason;
        FormValidationTestHelper.AssertInvalid(model, nameof(StockAdjustmentRequest.Reason));
    }

    [Fact]
    public void Reason_WhenTooLong_IsInvalid()
    {
        var model = ValidRequest();
        model.Reason = new string('x', 151);
        FormValidationTestHelper.AssertInvalid(model, nameof(StockAdjustmentRequest.Reason));
    }
}
