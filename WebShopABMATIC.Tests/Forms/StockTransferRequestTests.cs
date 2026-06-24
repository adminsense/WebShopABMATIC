using WebShopABMATIC.Application.Admin.Stock;

namespace WebShopABMATIC.Tests.Forms;

public class StockTransferRequestTests
{
    private static StockTransferRequest ValidRequest() => new()
    {
        ProductId = 100,
        FromStockLocationId = 1,
        ToStockLocationId = 2,
        Quantity = 5m,
        Reason = "Replenish shop floor"
    };

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        FormValidationTestHelper.AssertValid(ValidRequest());
    }

    [Fact]
    public void FromAndTo_WhenSameLocation_IsInvalid()
    {
        var model = ValidRequest();
        model.ToStockLocationId = model.FromStockLocationId;
        FormValidationTestHelper.AssertInvalid(model, nameof(StockTransferRequest.ToStockLocationId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quantity_WhenNotPositive_IsInvalid(decimal quantity)
    {
        var model = ValidRequest();
        model.Quantity = quantity;
        FormValidationTestHelper.AssertInvalid(model, nameof(StockTransferRequest.Quantity));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")]
    public void Reason_WhenTooShort_IsInvalid(string reason)
    {
        var model = ValidRequest();
        model.Reason = reason;
        FormValidationTestHelper.AssertInvalid(model, nameof(StockTransferRequest.Reason));
    }
}
