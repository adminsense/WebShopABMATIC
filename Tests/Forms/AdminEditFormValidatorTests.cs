using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Admin.CustomerTypes;
using WebShopABMATIC.Application.Admin.DeliveryTypes;
using WebShopABMATIC.Application.Admin.Manufacturers;
using WebShopABMATIC.Application.Admin.OrderStatuses;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Admin.PaymentMethods;
using WebShopABMATIC.Application.Admin.PriceListCategories;
using WebShopABMATIC.Application.Admin.ProductOptions;
using WebShopABMATIC.Application.Admin.ProductPrices;
using WebShopABMATIC.Application.Admin.ProductQuantityTiers;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Admin.StockLocations;
using WebShopABMATIC.Application.Admin.Suppliers;
using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Admin.UserGroups;
using WebShopABMATIC.Application.Admin.VatTypes;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;
using WebShopABMATIC.Application.Admin.WebshopStructures;
using WebShopABMATIC.Application.Validation;

namespace WebShopABMATIC.Tests.Forms;

public class AdminEditFormValidatorTests
{
    public static IEnumerable<object[]> ValidAdminForms() =>
    [
        [new ManufacturerEditDto { Name = "Acme" }],
        [new SupplierEditDto { Name = "Supplier", LanguageId = 1 }],
        [new StaffUserEditDto { Login = "staff", FirstName = "A", LastName = "B" }],
        [new ApplicationUserAccountEditDto { Email = "a@b.com", FirstName = "A", LastName = "B" }],
        [new CustomerEditDto
        {
            CustomerName = "Customer",
            CustomerEmail = "c@example.com",
            CustomerPhone = "123",
            CustomerTypeId = 1,
            CustomerCityId = 1
        }],
        [new CustomerTypeEditDto { CustomerTypeName = "Retail", CustomerTypeNameFr = "Détail" }],
        [new CustomerDeliveryAddressEditDto
        {
            Name = "Warehouse",
            Straat = "Street",
            Number = "1",
            CityId = 1
        }],
        [new CustomerProductDiscountEditDto
        {
            CustomerId = 1,
            ProductId = 1,
            FromAddress = DateTime.Today
        }],
        [new ProductEditDto { NameEn = "Product" }],
        [new ProductOptionEditDto
        {
            ProductId = 1,
            NameEn = "Color",
            ValueType = "text",
            Tag = "color"
        }],
        [new ProductPriceEditDto
        {
            ProductId = 1,
            FromAddress = DateTime.Today
        }],
        [new ProductQuantityTierEditDto
        {
            ProductId = 1,
            MinimumQuantity = 10
        }],
        [new OrderStatusEditDto { Name = "New", NameFr = "Nouveau", ScreenMode = "open" }],
        [new VatTypeEditDto { Name = "21%", InvoiceText = "VAT 21%" }],
        [new PaymentMethodEditDto { NameEn = "Card", NameNl = "Kaart", NameFr = "Carte" }],
        [new DeliveryTypeEditDto { Name = "Pickup", NameFr = "Retrait" }],
        [new UserGroupEditDto { Name = "Managers" }],
        [new StockLocationEditDto { Name = "Main" }],
        [new PriceListCategoryEditDto { Name = "Standard", NameFr = "Standard" }],
        [new WebshopStructureEditDto { NameNl = "Category" }],
        [new WebshopProductStructureEditDto { NameEn = "Group", NameNl = "Groep", NameFr = "Groupe" }],
        [new OrderEditDto
        {
            ProjectId = 1,
            CreatedAt = DateTime.Today,
            DeliveryTypeId = 1
        }]
    ];

    [Theory]
    [MemberData(nameof(ValidAdminForms))]
    public void ValidAdminForm_PassesValidation(object model)
    {
        Assert.True(AdminEditFormValidator.IsValid(model));
    }

    [Fact]
    public void Manufacturer_NameRequired()
    {
        var dto = new ManufacturerEditDto();
        Assert.False(AdminEditFormValidator.IsValid(dto));
        Assert.Contains(AdminEditFormValidator.Validate(dto), r => r.MemberNames.Contains(nameof(ManufacturerEditDto.Name)));
    }

    [Fact]
    public void Supplier_NameAndLanguageRequired()
    {
        var dto = new SupplierEditDto();
        var errors = AdminEditFormValidator.Validate(dto);
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(SupplierEditDto.Name)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(SupplierEditDto.LanguageId)));
    }

    [Fact]
    public void StaffUser_LoginAndNamesRequired()
    {
        var dto = new StaffUserEditDto();
        var errors = AdminEditFormValidator.Validate(dto);
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(StaffUserEditDto.Login)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(StaffUserEditDto.FirstName)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(StaffUserEditDto.LastName)));
    }

    [Fact]
    public void Customer_RequiredFields()
    {
        var dto = new CustomerEditDto();
        var errors = AdminEditFormValidator.Validate(dto);
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(CustomerEditDto.CustomerName)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(CustomerEditDto.CustomerEmail)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(CustomerEditDto.CustomerPhone)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(CustomerEditDto.CustomerTypeId)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(CustomerEditDto.CustomerCityId)));
    }

    [Fact]
    public void ProductOption_AllRequiredFields()
    {
        var dto = new ProductOptionEditDto();
        var errors = AdminEditFormValidator.Validate(dto);
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(ProductOptionEditDto.ProductId)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(ProductOptionEditDto.NameEn)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(ProductOptionEditDto.ValueType)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(ProductOptionEditDto.Tag)));
    }

    [Fact]
    public void CustomerDiscount_FromAddressRequired()
    {
        var dto = new CustomerProductDiscountEditDto { CustomerId = 1, ProductId = 1 };
        var errors = AdminEditFormValidator.Validate(dto);
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(CustomerProductDiscountEditDto.FromAddress)));
    }

    [Fact]
    public void Order_CreateMode_RequiresProjectAndDates()
    {
        var dto = new OrderEditDto { DeliveryTypeId = 0 };
        var errors = AdminEditFormValidator.Validate(dto);
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(OrderEditDto.ProjectId)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(OrderEditDto.CreatedAt)));
        Assert.Contains(errors, r => r.MemberNames.Contains(nameof(OrderEditDto.DeliveryTypeId)));
    }

    [Fact]
    public void Order_EditMode_SkipsProjectId()
    {
        var dto = new OrderEditDto
        {
            Id = 42,
            CreatedAt = DateTime.Today,
            DeliveryTypeId = 1
        };
        Assert.True(AdminEditFormValidator.IsValid(dto));
    }

    [Fact]
    public void UnknownModel_ReturnsNoErrors()
    {
        Assert.True(AdminEditFormValidator.IsValid(new object()));
    }
}
