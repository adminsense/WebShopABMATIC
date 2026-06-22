using Microsoft.Extensions.DependencyInjection;
using Moq;
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
using WebShopABMATIC.Application.Admin.ProductStockLocations;
using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Admin.StockLocations;
using WebShopABMATIC.Application.Admin.Suppliers;
using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Admin.UserGroups;
using WebShopABMATIC.Application.Admin.VatTypes;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;
using WebShopABMATIC.Application.Admin.WebshopStructures;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Web.Services;

namespace WebShopABMATIC.Tests.Razor;

internal static class AdminPortMockRegistrar
{
    public static void Register(IServiceCollection services, Type componentType)
    {
        services.AddSingleton(Mock.Of<IGridExportService>());

        switch (componentType.Name)
        {
            case "ManufacturerList": RegisterManufacturer(services); break;
            case "SupplierList": RegisterSupplier(services); break;
            case "StaffUserList": RegisterStaffUser(services); break;
            case "UserAccountList": RegisterUserAccount(services); break;
            case "CustomerList": RegisterCustomer(services); break;
            case "CustomerTypeList": RegisterCustomerType(services); break;
            case "DeliveryAddressList": RegisterDeliveryAddress(services); break;
            case "CustomerDiscountList": RegisterCustomerDiscount(services); break;
            case "ProductList": RegisterProduct(services); break;
            case "ProductOptionList": RegisterProductOption(services); break;
            case "ProductPriceList": RegisterProductPrice(services); break;
            case "ProductTierList": RegisterProductTier(services); break;
            case "OrderStatusList": RegisterOrderStatus(services); break;
            case "VatTypeList": RegisterVatType(services); break;
            case "PaymentMethodList": RegisterPaymentMethod(services); break;
            case "DeliveryTypeList": RegisterDeliveryType(services); break;
            case "UserGroupList": RegisterUserGroup(services); break;
            case "StockLocationList": RegisterStockLocation(services); break;
            case "PriceListCategoryList": RegisterPriceListCategory(services); break;
            case "WebshopStructureList": RegisterWebshopStructure(services); break;
            case "WebshopProductStructureList": RegisterWebshopProductStructure(services); break;
            case "OrderList": RegisterOrder(services); break;
            case "ProductStockList": RegisterProductStock(services); break;
            case "StockAdjustment": RegisterStockAdjustment(services); break;
            default:
                throw new ArgumentException($"No admin port mock registered for {componentType.Name}.", nameof(componentType));
        }
    }

    private static PagedResult<T> Empty<T>() => new() { Items = [], TotalCount = 0, Page = 1, PageSize = 20 };

    private static void RegisterManufacturer(IServiceCollection services)
    {
        var mock = new Mock<IManufacturerAdminPort>();
        mock.Setup(p => p.GetManufacturersAsync(It.IsAny<ManufacturerListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ManufacturerDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterSupplier(IServiceCollection services)
    {
        var mock = new Mock<ISupplierAdminPort>();
        mock.Setup(p => p.GetSuppliersAsync(It.IsAny<SupplierListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<SupplierDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterStaffUser(IServiceCollection services)
    {
        var mock = new Mock<IStaffUserAdminPort>();
        mock.Setup(p => p.GetStaffUsersAsync(It.IsAny<StaffUserListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<StaffUserDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterUserAccount(IServiceCollection services)
    {
        var mock = new Mock<IApplicationUserAccountAdminPort>();
        mock.Setup(p => p.GetAccountsAsync(It.IsAny<ApplicationUserAccountListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ApplicationUserAccountDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterCustomer(IServiceCollection services)
    {
        var mock = new Mock<ICustomerAdminPort>();
        mock.Setup(p => p.GetCustomersAsync(It.IsAny<CustomerListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<CustomerDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterCustomerType(IServiceCollection services)
    {
        var mock = new Mock<ICustomerTypeAdminPort>();
        mock.Setup(p => p.GetCustomerTypesAsync(It.IsAny<CustomerTypeListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<CustomerTypeDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterDeliveryAddress(IServiceCollection services)
    {
        var mock = new Mock<ICustomerDeliveryAddressAdminPort>();
        mock.Setup(p => p.GetCustomerDeliveryAddressesAsync(It.IsAny<CustomerDeliveryAddressListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<CustomerDeliveryAddressDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterCustomerDiscount(IServiceCollection services)
    {
        var mock = new Mock<ICustomerProductDiscountAdminPort>();
        mock.Setup(p => p.GetCustomerProductDiscountsAsync(It.IsAny<CustomerProductDiscountListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<CustomerProductDiscountDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterProduct(IServiceCollection services)
    {
        var mock = new Mock<IProductAdminPort>();
        mock.Setup(p => p.GetProductsAsync(It.IsAny<ProductListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ProductDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterProductOption(IServiceCollection services)
    {
        var mock = new Mock<IProductOptionAdminPort>();
        mock.Setup(p => p.GetProductOptionsAsync(It.IsAny<ProductOptionListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ProductOptionDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterProductPrice(IServiceCollection services)
    {
        var mock = new Mock<IProductPriceAdminPort>();
        mock.Setup(p => p.GetProductPricesAsync(It.IsAny<ProductPriceListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ProductPriceDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterProductTier(IServiceCollection services)
    {
        var mock = new Mock<IProductQuantityTierAdminPort>();
        mock.Setup(p => p.GetProductQuantityTiersAsync(It.IsAny<ProductQuantityTierListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ProductQuantityTierDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterOrderStatus(IServiceCollection services)
    {
        var mock = new Mock<IOrderStatusAdminPort>();
        mock.Setup(p => p.GetOrderStatusesAsync(It.IsAny<OrderStatusListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<OrderStatusDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterVatType(IServiceCollection services)
    {
        var mock = new Mock<IVatTypeAdminPort>();
        mock.Setup(p => p.GetVatTypesAsync(It.IsAny<VatTypeListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<VatTypeDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterPaymentMethod(IServiceCollection services)
    {
        var mock = new Mock<IPaymentMethodAdminPort>();
        mock.Setup(p => p.GetPaymentMethodsAsync(It.IsAny<PaymentMethodListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<PaymentMethodDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterDeliveryType(IServiceCollection services)
    {
        var mock = new Mock<IDeliveryTypeAdminPort>();
        mock.Setup(p => p.GetDeliveryTypesAsync(It.IsAny<DeliveryTypeListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<DeliveryTypeDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterUserGroup(IServiceCollection services)
    {
        var mock = new Mock<IUserGroupAdminPort>();
        mock.Setup(p => p.GetUserGroupsAsync(It.IsAny<UserGroupListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<UserGroupDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterStockLocation(IServiceCollection services)
    {
        var mock = new Mock<IStockLocationAdminPort>();
        mock.Setup(p => p.GetStockLocationsAsync(It.IsAny<StockLocationListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<StockLocationDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterPriceListCategory(IServiceCollection services)
    {
        var mock = new Mock<IPriceListCategoryAdminPort>();
        mock.Setup(p => p.GetPriceListCategoriesAsync(It.IsAny<PriceListCategoryListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<PriceListCategoryDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterWebshopStructure(IServiceCollection services)
    {
        var mock = new Mock<IWebshopStructureAdminPort>();
        mock.Setup(p => p.GetWebshopStructuresAsync(It.IsAny<WebshopStructureListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<WebshopStructureDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterWebshopProductStructure(IServiceCollection services)
    {
        var mock = new Mock<IWebshopProductStructureAdminPort>();
        mock.Setup(p => p.GetWebshopProductStructuresAsync(It.IsAny<WebshopProductStructureListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<WebshopProductStructureDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterOrder(IServiceCollection services)
    {
        var mock = new Mock<IOrderAdminPort>();
        mock.Setup(p => p.GetOrdersAsync(It.IsAny<OrderListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<OrderSummaryDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterProductStock(IServiceCollection services)
    {
        var mock = new Mock<IProductStockLocationAdminPort>();
        mock.Setup(p => p.GetProductStockLocationsAsync(It.IsAny<ProductStockLocationListFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Empty<ProductStockLocationDto>());
        services.AddSingleton(mock.Object);
    }

    private static void RegisterStockAdjustment(IServiceCollection services)
    {
        var mock = new Mock<IStockAdjustmentPort>();
        mock.Setup(p => p.GetLookupsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StockAdjustmentLookupsDto
            {
                StockLocations = [new StockLookupItemDto { Id = 1, Name = "Main" }]
            });
        services.AddSingleton(mock.Object);
    }
}
