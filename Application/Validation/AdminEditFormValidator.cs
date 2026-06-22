using System.ComponentModel.DataAnnotations;
using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Admin.CustomerTypes;
using WebShopABMATIC.Application.Admin.DeliveryTypes;
using WebShopABMATIC.Application.Admin.Manufacturers;
using WebShopABMATIC.Application.Admin.OrderStatuses;
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
using WebShopABMATIC.Application.Admin.ProductStockLocations;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;
using WebShopABMATIC.Application.Admin.WebshopStructures;

namespace WebShopABMATIC.Application.Validation;

/// <summary>
/// Server-side validation aligned with admin form required fields in Razor pages.
/// </summary>
public static class AdminEditFormValidator
{
    public static IReadOnlyList<ValidationResult> Validate(object model) =>
        model switch
        {
            ManufacturerEditDto dto => ValidateManufacturer(dto),
            SupplierEditDto dto => ValidateSupplier(dto),
            StaffUserEditDto dto => ValidateStaffUser(dto),
            ApplicationUserAccountEditDto dto => ValidateUserAccount(dto),
            CustomerEditDto dto => ValidateCustomer(dto),
            CustomerTypeEditDto dto => ValidateCustomerType(dto),
            CustomerDeliveryAddressEditDto dto => ValidateDeliveryAddress(dto),
            CustomerProductDiscountEditDto dto => ValidateCustomerDiscount(dto),
            ProductEditDto dto => ValidateProduct(dto),
            ProductOptionEditDto dto => ValidateProductOption(dto),
            ProductPriceEditDto dto => ValidateProductPrice(dto),
            ProductQuantityTierEditDto dto => ValidateProductTier(dto),
            OrderStatusEditDto dto => ValidateOrderStatus(dto),
            VatTypeEditDto dto => ValidateVatType(dto),
            PaymentMethodEditDto dto => ValidatePaymentMethod(dto),
            DeliveryTypeEditDto dto => ValidateDeliveryType(dto),
            UserGroupEditDto dto => ValidateUserGroup(dto),
            StockLocationEditDto dto => ValidateStockLocation(dto),
            PriceListCategoryEditDto dto => ValidatePriceListCategory(dto),
            WebshopStructureEditDto dto => ValidateWebshopStructure(dto),
            WebshopProductStructureEditDto dto => ValidateWebshopProductStructure(dto),
            OrderEditDto dto => ValidateOrder(dto),
            ProductStockLocationEditDto dto => ValidateProductStockLocation(dto),
            _ => []
        };

    public static bool IsValid(object model) => Validate(model).Count == 0;

    private static List<ValidationResult> RequiredString(string? value, string member) =>
        string.IsNullOrWhiteSpace(value)
            ? [new ValidationResult("The field is required.", [member])]
            : [];

    private static List<ValidationResult> RequiredPositiveInt(int value, string member) =>
        value <= 0
            ? [new ValidationResult("The field must be greater than zero.", [member])]
            : [];

    private static List<ValidationResult> ValidateManufacturer(ManufacturerEditDto dto) =>
        RequiredString(dto.Name, nameof(dto.Name));

    private static List<ValidationResult> ValidateSupplier(SupplierEditDto dto)
    {
        var errors = RequiredString(dto.Name, nameof(dto.Name));
        errors.AddRange(RequiredPositiveInt(dto.LanguageId, nameof(dto.LanguageId)));
        return errors;
    }

    private static List<ValidationResult> ValidateStaffUser(StaffUserEditDto dto)
    {
        var errors = RequiredString(dto.Login, nameof(dto.Login));
        errors.AddRange(RequiredString(dto.FirstName, nameof(dto.FirstName)));
        errors.AddRange(RequiredString(dto.LastName, nameof(dto.LastName)));
        return errors;
    }

    private static List<ValidationResult> ValidateUserAccount(ApplicationUserAccountEditDto dto)
    {
        var errors = RequiredString(dto.Email, nameof(dto.Email));
        errors.AddRange(RequiredString(dto.FirstName, nameof(dto.FirstName)));
        errors.AddRange(RequiredString(dto.LastName, nameof(dto.LastName)));
        return errors;
    }

    private static List<ValidationResult> ValidateCustomer(CustomerEditDto dto)
    {
        var errors = RequiredString(dto.CustomerName, nameof(dto.CustomerName));
        errors.AddRange(RequiredString(dto.CustomerEmail, nameof(dto.CustomerEmail)));
        errors.AddRange(RequiredString(dto.CustomerPhone, nameof(dto.CustomerPhone)));
        errors.AddRange(RequiredPositiveInt(dto.CustomerTypeId, nameof(dto.CustomerTypeId)));
        errors.AddRange(RequiredPositiveInt(dto.CustomerCityId, nameof(dto.CustomerCityId)));
        return errors;
    }

    private static List<ValidationResult> ValidateCustomerType(CustomerTypeEditDto dto)
    {
        var errors = RequiredString(dto.CustomerTypeName, nameof(dto.CustomerTypeName));
        errors.AddRange(RequiredString(dto.CustomerTypeNameFr, nameof(dto.CustomerTypeNameFr)));
        return errors;
    }

    private static List<ValidationResult> ValidateDeliveryAddress(CustomerDeliveryAddressEditDto dto)
    {
        var errors = RequiredString(dto.Name, nameof(dto.Name));
        errors.AddRange(RequiredString(dto.Straat, nameof(dto.Straat)));
        errors.AddRange(RequiredString(dto.Number, nameof(dto.Number)));
        errors.AddRange(RequiredPositiveInt(dto.CityId, nameof(dto.CityId)));
        return errors;
    }

    private static List<ValidationResult> ValidateCustomerDiscount(CustomerProductDiscountEditDto dto)
    {
        var errors = RequiredPositiveInt(dto.CustomerId, nameof(dto.CustomerId));
        errors.AddRange(RequiredPositiveInt(dto.ProductId, nameof(dto.ProductId)));
        if (dto.FromAddress == default)
        {
            errors.Add(new ValidationResult("The field is required.", [nameof(dto.FromAddress)]));
        }

        return errors;
    }

    private static List<ValidationResult> ValidateProduct(ProductEditDto dto) =>
        RequiredString(dto.NameEn, nameof(dto.NameEn));

    private static List<ValidationResult> ValidateProductOption(ProductOptionEditDto dto)
    {
        var errors = RequiredPositiveInt(dto.ProductId, nameof(dto.ProductId));
        errors.AddRange(RequiredString(dto.NameEn, nameof(dto.NameEn)));
        errors.AddRange(RequiredString(dto.ValueType, nameof(dto.ValueType)));
        errors.AddRange(RequiredString(dto.Tag, nameof(dto.Tag)));
        return errors;
    }

    private static List<ValidationResult> ValidateProductPrice(ProductPriceEditDto dto)
    {
        var errors = RequiredPositiveInt(dto.ProductId, nameof(dto.ProductId));
        if (dto.FromAddress == default)
        {
            errors.Add(new ValidationResult("The field is required.", [nameof(dto.FromAddress)]));
        }

        return errors;
    }

    private static List<ValidationResult> ValidateProductTier(ProductQuantityTierEditDto dto)
    {
        var errors = RequiredPositiveInt(dto.ProductId, nameof(dto.ProductId));
        if (dto.MinimumQuantity <= 0)
        {
            errors.Add(new ValidationResult("The field must be greater than zero.", [nameof(dto.MinimumQuantity)]));
        }

        return errors;
    }

    private static List<ValidationResult> ValidateOrderStatus(OrderStatusEditDto dto)
    {
        var errors = RequiredString(dto.Name, nameof(dto.Name));
        errors.AddRange(RequiredString(dto.NameFr, nameof(dto.NameFr)));
        errors.AddRange(RequiredString(dto.ScreenMode, nameof(dto.ScreenMode)));
        return errors;
    }

    private static List<ValidationResult> ValidateVatType(VatTypeEditDto dto)
    {
        var errors = RequiredString(dto.Name, nameof(dto.Name));
        errors.AddRange(RequiredString(dto.InvoiceText, nameof(dto.InvoiceText)));
        return errors;
    }

    private static List<ValidationResult> ValidatePaymentMethod(PaymentMethodEditDto dto)
    {
        var errors = RequiredString(dto.NameEn, nameof(dto.NameEn));
        errors.AddRange(RequiredString(dto.NameNl, nameof(dto.NameNl)));
        errors.AddRange(RequiredString(dto.NameFr, nameof(dto.NameFr)));
        return errors;
    }

    private static List<ValidationResult> ValidateDeliveryType(DeliveryTypeEditDto dto)
    {
        var errors = RequiredString(dto.Name, nameof(dto.Name));
        errors.AddRange(RequiredString(dto.NameFr, nameof(dto.NameFr)));
        return errors;
    }

    private static List<ValidationResult> ValidateUserGroup(UserGroupEditDto dto) =>
        RequiredString(dto.Name, nameof(dto.Name));

    private static List<ValidationResult> ValidateStockLocation(StockLocationEditDto dto) =>
        RequiredString(dto.Name, nameof(dto.Name));

    private static List<ValidationResult> ValidatePriceListCategory(PriceListCategoryEditDto dto)
    {
        var errors = RequiredString(dto.Name, nameof(dto.Name));
        errors.AddRange(RequiredString(dto.NameFr, nameof(dto.NameFr)));
        return errors;
    }

    private static List<ValidationResult> ValidateWebshopStructure(WebshopStructureEditDto dto) =>
        RequiredString(dto.NameNl, nameof(dto.NameNl));

    private static List<ValidationResult> ValidateWebshopProductStructure(WebshopProductStructureEditDto dto)
    {
        var errors = RequiredString(dto.NameEn, nameof(dto.NameEn));
        errors.AddRange(RequiredString(dto.NameNl, nameof(dto.NameNl)));
        errors.AddRange(RequiredString(dto.NameFr, nameof(dto.NameFr)));
        return errors;
    }

    private static List<ValidationResult> ValidateOrder(OrderEditDto dto)
    {
        var errors = new List<ValidationResult>();
        if (dto.Id <= 0)
        {
            errors.AddRange(RequiredPositiveInt(dto.ProjectId, nameof(dto.ProjectId)));
        }

        if (dto.CreatedAt == default)
        {
            errors.Add(new ValidationResult("The field is required.", [nameof(dto.CreatedAt)]));
        }

        errors.AddRange(RequiredPositiveInt(dto.DeliveryTypeId, nameof(dto.DeliveryTypeId)));
        return errors;
    }

    private static List<ValidationResult> ValidateProductStockLocation(ProductStockLocationEditDto dto)
    {
        var errors = RequiredPositiveInt(dto.StockLocationId, nameof(dto.StockLocationId));
        errors.AddRange(RequiredPositiveInt(dto.ProductId, nameof(dto.ProductId)));
        return errors;
    }
}
