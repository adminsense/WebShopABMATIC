using WebShopABMATIC.Web.Components.Pages.Admin;

namespace WebShopABMATIC.Tests.Razor;

public class AdminEntityListFormRazorTests
{
    public static TheoryData<Type, string> AdminFormPages => new()
    {
        { typeof(ManufacturerList), "f-name" },
        { typeof(SupplierList), "f-name" },
        { typeof(StaffUserList), "f-login" },
        { typeof(UserAccountList), "f-email" },
        { typeof(CustomerList), "f-customername" },
        { typeof(CustomerTypeList), "f-customertypename" },
        { typeof(DeliveryAddressList), "f-name" },
        { typeof(CustomerDiscountList), "f-customerid" },
        { typeof(ProductList), "prod-name" },
        { typeof(ProductOptionList), "f-productid" },
        { typeof(ProductPriceList), "f-productid" },
        { typeof(ProductTierList), "f-productid" },
        { typeof(OrderStatusList), "f-name" },
        { typeof(VatTypeList), "f-name" },
        { typeof(PaymentMethodList), "f-nameen" },
        { typeof(DeliveryTypeList), "f-name" },
        { typeof(UserGroupList), "f-name" },
        { typeof(StockLocationList), "f-name" },
        { typeof(PriceListCategoryList), "f-name" },
        { typeof(WebshopStructureList), "f-namenl" },
        { typeof(WebshopProductStructureList), "f-nameen" },
        { typeof(OrderList), "f-projectid" },
        { typeof(ProductStockList), "f-stocklocationid" },
        { typeof(StockAdjustment), "adj-product" },
        { typeof(StockTransfer), "xfer-product" }
    };

    [Theory]
    [MemberData(nameof(AdminFormPages))]
    public void AdminFormPage_RendersEditFormWithRequiredField(Type componentType, string requiredFieldId)
    {
        using var ctx = new BlazorFormTestContext(asAdmin: true);
        AdminPortMockRegistrar.Register(ctx.Services, componentType);

        var cut = ctx.RenderFormComponent(componentType);

        cut.WaitForAssertion(() =>
        {
            Assert.NotNull(cut.Find($"#{requiredFieldId}"));
            Assert.NotEmpty(cut.FindAll("button[type=submit]"));
        });
    }
}
