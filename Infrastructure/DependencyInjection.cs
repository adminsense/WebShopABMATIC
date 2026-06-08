using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Admin;
using WebShopABMATIC.Infrastructure.Audit;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Infrastructure.Media;
using WebShopABMATIC.Infrastructure.Notifications;
using WebShopABMATIC.Infrastructure.Payments;
using WebShopABMATIC.Infrastructure.Persistence.Repositories;
using WebShopABMATIC.Infrastructure.Stock;
using WebShopABMATIC.Infrastructure.Store;

namespace WebShopABMATIC.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebShopInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var identityConnection = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");

        var domainConnection = configuration.GetConnectionString("connWebShopABMATIC")
            ?? identityConnection;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(identityConnection));

        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<IAuditSuppressionContext, AuditSuppressionContext>();
        services.AddDbContext<WebShopABMATICDbContext>((serviceProvider, options) =>
            options.UseSqlServer(domainConnection)
                .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));

        // Driving adapters (configuration / external systems)
        services.AddSingleton<IAdminHubPort, AdminHubRegistry>();
        services.AddScoped<IProductMediaPort, LocalProductMediaService>();
        services.AddScoped<IStoreCatalogPort, StoreCatalogService>();

        // Driven adapters (persistence / outbound ports)
        services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IWebshopStructureRepository, WebshopStructureRepository>();
        services.AddScoped<IWebshopProductStructureRepository, WebshopProductStructureRepository>();
        services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
        services.AddScoped<IProductPricingPort, ProductPricingRepository>();
        services.AddScoped<IProductQuantityTierRepository, ProductQuantityTierRepository>();
        services.AddScoped<IProductOptionRepository, ProductOptionRepository>();
        services.AddScoped<IPriceListCategoryRepository, PriceListCategoryRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ICustomerDeliveryAddressRepository, CustomerDeliveryAddressRepository>();
        services.AddScoped<ICustomerProductDiscountRepository, CustomerProductDiscountRepository>();
        services.AddScoped<ICustomerTypeRepository, CustomerTypeRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
        services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductStockLocationRepository, ProductStockLocationRepository>();
        services.AddScoped<IStockLocationRepository, StockLocationRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<IStaffUserRepository, StaffUserRepository>();
        services.AddScoped<ISystemUserRepository, SystemUserRepository>();
        services.AddScoped<IApplicationUserAccountRepository, ApplicationUserAccountRepository>();
        services.AddScoped<IIdentityPasswordPort, IdentityPasswordService>();
        services.AddScoped<ICurrentUserContext, HttpCurrentUserContext>();
        services.AddScoped<ICustomerRegistrationRepository, CustomerRegistrationRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IVatTypeRepository, VatTypeRepository>();
        services.AddScoped<IStockOverviewRepository, StockOverviewRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<ILowStockReadRepository, LowStockReadRepository>();
        services.AddScoped<ILowStockAlertService, LowStockAlertService>();
        services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddSingleton<IManualLogoutTracker, ManualLogoutTracker>();
        services.AddScoped<IStoreCustomerRepository, StoreCustomerRepository>();
        services.AddScoped<IStoreProfileRepository, StoreProfileRepository>();
        services.AddScoped<IStoreOrderRepository, StoreOrderRepository>();
        services.AddScoped<IStockMovementService, StockMovementService>();

        services.AddWebShopMollie(configuration, environment);
        services.AddWebShopNotifications(configuration, environment);

        return services;
    }
}
