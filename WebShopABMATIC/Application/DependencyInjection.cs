using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.UseCases.Admin;
using WebShopABMATIC.Application.UseCases.Store;

namespace WebShopABMATIC.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddWebShopApplication(this IServiceCollection services)
    {
        services.AddScoped<IAdminDashboardPort, AdminDashboardUseCase>();
        services.AddScoped<IProductAdminPort, ProductAdminUseCase>();

        services.AddScoped<IWebshopStructureAdminPort, WebshopStructureAdminUseCase>();
        services.AddScoped<IWebshopProductStructureAdminPort, WebshopProductStructureAdminUseCase>();
        services.AddScoped<IProductPriceAdminPort, ProductPriceAdminUseCase>();
        services.AddScoped<IProductQuantityTierAdminPort, ProductQuantityTierAdminUseCase>();
        services.AddScoped<IProductOptionAdminPort, ProductOptionAdminUseCase>();
        services.AddScoped<IPriceListCategoryAdminPort, PriceListCategoryAdminUseCase>();
        services.AddScoped<IManufacturerAdminPort, ManufacturerAdminUseCase>();
        services.AddScoped<ISupplierAdminPort, SupplierAdminUseCase>();
        services.AddScoped<ICustomerDeliveryAddressAdminPort, CustomerDeliveryAddressAdminUseCase>();
        services.AddScoped<ICustomerProductDiscountAdminPort, CustomerProductDiscountAdminUseCase>();
        services.AddScoped<ICustomerTypeAdminPort, CustomerTypeAdminUseCase>();
        services.AddScoped<ICustomerAdminPort, CustomerAdminUseCase>();
        services.AddScoped<IOrderStatusAdminPort, OrderStatusAdminUseCase>();
        services.AddScoped<IDeliveryTypeAdminPort, DeliveryTypeAdminUseCase>();
        services.AddScoped<IOrderAdminPort, OrderAdminUseCase>();
        services.AddScoped<IProductStockLocationAdminPort, ProductStockLocationAdminUseCase>();
        services.AddScoped<IStockLocationAdminPort, StockLocationAdminUseCase>();
        services.AddScoped<IPaymentMethodAdminPort, PaymentMethodAdminUseCase>();
        services.AddScoped<IStaffUserAdminPort, StaffUserAdminUseCase>();
        services.AddScoped<IUserGroupAdminPort, UserGroupAdminUseCase>();
        services.AddScoped<IVatTypeAdminPort, VatTypeAdminUseCase>();
        services.AddScoped<IStockOverviewPort, StockOverviewUseCase>();
        services.AddScoped<IStockMovementAdminPort, StockMovementAdminUseCase>();
        services.AddScoped<IStockAdjustmentPort, StockAdjustmentUseCase>();
        services.AddScoped<IStockTransferPort, StockTransferUseCase>();

        services.AddScoped<ICheckoutPort, CheckoutUseCase>();
        services.AddScoped<ICustomerRegistrationPort, CustomerRegistrationUseCase>();
        services.AddScoped<IStoreProfilePort, StoreProfileUseCase>();
        services.AddScoped<IMollieWebhookPort, ProcessMollieWebhookUseCase>();

        return services;
    }
}
