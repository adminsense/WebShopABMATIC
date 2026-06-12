using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Admin.CustomerTypes;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Admin.DeliveryTypes;
using WebShopABMATIC.Application.Admin.Hubs;
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
using WebShopABMATIC.Application.Admin.SystemUsers;
using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Admin.StockLocations;
using WebShopABMATIC.Application.Admin.Suppliers;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.Admin.UserGroups;
using WebShopABMATIC.Application.Admin.VatTypes;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;
using WebShopABMATIC.Application.Admin.WebshopStructures;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports;

// Inbound (driving) ports — called by Blazor UI adapters.

public interface IAdminDashboardPort
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task MarkStockAlertsReadAsync(CancellationToken cancellationToken = default);
}

public interface IAdminHubPort
{
    IReadOnlyList<AdminHubDefinitionDto> GetHubDefinitions();
    AdminHubDefinitionDto? GetHub(string hubId);
}

public interface IProductAdminPort
{
    Task<PagedResult<ProductDto>> GetProductsAsync(ProductListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductEditDto dto, ProductImageUpload? primaryImage, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int productId, CancellationToken cancellationToken = default);
}

public interface IWebshopStructureAdminPort
{
    Task<PagedResult<WebshopStructureDto>> GetWebshopStructuresAsync(WebshopStructureListFilter filter, CancellationToken cancellationToken = default);
    Task<WebshopStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(WebshopStructureEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IWebshopProductStructureAdminPort
{
    Task<PagedResult<WebshopProductStructureDto>> GetWebshopProductStructuresAsync(WebshopProductStructureListFilter filter, CancellationToken cancellationToken = default);
    Task<WebshopProductStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(WebshopProductStructureEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IProductPriceAdminPort
{
    Task<PagedResult<ProductPriceDto>> GetProductPricesAsync(ProductPriceListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductPriceEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductPriceEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IProductQuantityTierAdminPort
{
    Task<PagedResult<ProductQuantityTierDto>> GetProductQuantityTiersAsync(ProductQuantityTierListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductQuantityTierEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductQuantityTierEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IProductOptionAdminPort
{
    Task<PagedResult<ProductOptionDto>> GetProductOptionsAsync(ProductOptionListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductOptionEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductOptionEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IPriceListCategoryAdminPort
{
    Task<PagedResult<PriceListCategoryDto>> GetPriceListCategoriesAsync(PriceListCategoryListFilter filter, CancellationToken cancellationToken = default);
    Task<PriceListCategoryEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(PriceListCategoryEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IManufacturerAdminPort
{
    Task<PagedResult<ManufacturerDto>> GetManufacturersAsync(ManufacturerListFilter filter, CancellationToken cancellationToken = default);
    Task<ManufacturerEditDto?> GetForEditAsync(int manufacturerId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ManufacturerEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int manufacturerId, CancellationToken cancellationToken = default);
}

public interface ISupplierAdminPort
{
    Task<PagedResult<SupplierDto>> GetSuppliersAsync(SupplierListFilter filter, CancellationToken cancellationToken = default);
    Task<SupplierEditDto?> GetForEditAsync(int supplierId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(SupplierEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int supplierId, CancellationToken cancellationToken = default);
}

public interface ICustomerDeliveryAddressAdminPort
{
    Task<PagedResult<CustomerDeliveryAddressDto>> GetCustomerDeliveryAddressesAsync(CustomerDeliveryAddressListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerDeliveryAddressEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerDeliveryAddressEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface ICustomerProductDiscountAdminPort
{
    Task<PagedResult<CustomerProductDiscountDto>> GetCustomerProductDiscountsAsync(CustomerProductDiscountListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerProductDiscountEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerProductDiscountEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface ICustomerTypeAdminPort
{
    Task<PagedResult<CustomerTypeDto>> GetCustomerTypesAsync(CustomerTypeListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerTypeEditDto?> GetForEditAsync(int klantTypeId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerTypeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int klantTypeId, CancellationToken cancellationToken = default);
}

public interface ICustomerAdminPort
{
    Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default);
    Task<CustomerEditDto?> GetForEditAsync(int customerId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(CustomerEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default);
    Task<PasswordResetResult> ResetWebshopPasswordAsync(int customerId, string? newPassword = null, CancellationToken cancellationToken = default);
}

public interface IOrderStatusAdminPort
{
    Task<PagedResult<OrderStatusDto>> GetOrderStatusesAsync(OrderStatusListFilter filter, CancellationToken cancellationToken = default);
    Task<OrderStatusEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(OrderStatusEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IDeliveryTypeAdminPort
{
    Task<PagedResult<DeliveryTypeDto>> GetDeliveryTypesAsync(DeliveryTypeListFilter filter, CancellationToken cancellationToken = default);
    Task<DeliveryTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(DeliveryTypeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IOrderAdminPort
{
    Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default);
    Task<OrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(OrderEditDto dto, CancellationToken cancellationToken = default);
}

public interface IProductStockLocationAdminPort
{
    Task<PagedResult<ProductStockLocationDto>> GetProductStockLocationsAsync(ProductStockLocationListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductStockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductStockLocationEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IStockLocationAdminPort
{
    Task<PagedResult<StockLocationDto>> GetStockLocationsAsync(StockLocationListFilter filter, CancellationToken cancellationToken = default);
    Task<StockLocationEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(StockLocationEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IPaymentMethodAdminPort
{
    Task<PagedResult<PaymentMethodDto>> GetPaymentMethodsAsync(PaymentMethodListFilter filter, CancellationToken cancellationToken = default);
    Task<PaymentMethodEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(PaymentMethodEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IStaffUserAdminPort
{
    Task<PagedResult<StaffUserDto>> GetStaffUsersAsync(StaffUserListFilter filter, CancellationToken cancellationToken = default);
    Task<StaffUserEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(StaffUserEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface ISystemUserAdminPort
{
    Task<PagedResult<SystemUserDto>> GetSystemUsersAsync(SystemUserListFilter filter, CancellationToken cancellationToken = default);
    Task<SystemUserEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default);
    Task<SystemUserSaveResult> SaveAsync(SystemUserEditDto dto, CancellationToken cancellationToken = default);

    Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default);
}

public interface IApplicationUserAccountAdminPort
{
    Task<PagedResult<ApplicationUserAccountDto>> GetAccountsAsync(ApplicationUserAccountListFilter filter, CancellationToken cancellationToken = default);
    Task<ApplicationUserAccountEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default);
    Task<ApplicationUserAccountSaveResult> SaveAsync(ApplicationUserAccountEditDto dto, CancellationToken cancellationToken = default);
    Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default);
}

public interface IUserGroupAdminPort
{
    Task<PagedResult<UserGroupDto>> GetUserGroupsAsync(UserGroupListFilter filter, CancellationToken cancellationToken = default);
    Task<UserGroupEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(UserGroupEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IVatTypeAdminPort
{
    Task<PagedResult<VatTypeDto>> GetVatTypesAsync(VatTypeListFilter filter, CancellationToken cancellationToken = default);
    Task<VatTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(VatTypeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

public interface IStockOverviewPort
{
    Task<StockOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default);
}

public interface IStockMovementAdminPort
{
    Task<PagedResult<StockMovementDto>> GetMovementsAsync(StockMovementListFilter filter, CancellationToken cancellationToken = default);
}

public interface IStockAdjustmentPort
{
    Task<StockAdjustmentLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default);
    Task<StockAdjustmentPreviewDto?> GetPreviewAsync(int productId, int stockLocationId, CancellationToken cancellationToken = default);
    Task<StockApplyResult> ApplyAsync(StockAdjustmentRequest request, CancellationToken cancellationToken = default);
}

public interface IAuditLogAdminPort
{
    Task<PagedResult<AuditLogListItemDto>> GetAuditLogsAsync(AuditLogListFilter filter, CancellationToken cancellationToken = default);
    Task<AuditLogDetailDto?> GetAuditLogDetailAsync(long id, CancellationToken cancellationToken = default);
}
