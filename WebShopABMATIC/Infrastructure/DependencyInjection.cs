using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Admin;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Infrastructure.Media;
using WebShopABMATIC.Infrastructure.Notifications;
using WebShopABMATIC.Infrastructure.Payments;
using WebShopABMATIC.Infrastructure.Persistence.Repositories;
using WebShopABMATIC.Infrastructure.Stock;
using WebShopABMATIC.Infrastructure.Store;
using WebShopABMATIC.Infrastructure.Audit;

namespace WebShopABMATIC.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebShopInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var domainConnection = configuration.GetConnectionString("connWebShopABMATIC")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'connWebShopABMATIC' or 'DefaultConnection' is required.");

        services.AddDbContext<WebShopABMATICDbContext>(options =>
            options.UseSqlServer(domainConnection, sql => sql.CommandTimeout(30)));

        services.AddScoped<ILegacySignInPort, LegacySignInService>();
        services.AddScoped<ILegacyCustomerPasswordPort, LegacyCustomerPasswordService>();
        services.AddScoped<ILegacyStaffProfilePort, LegacyStaffProfileService>();
        services.AddScoped<IAuditSuppressionContext, AuditSuppressionContext>();

        services.AddSingleton<IAdminHubPort, AdminHubRegistry>();
        services.AddProductMedia(configuration);
        services.AddScoped<StoreDbGate>();
        services.Configure<StoreCatalogFilterOptions>(
            configuration.GetSection(StoreCatalogFilterOptions.SectionName));
        services.AddScoped<IStoreCatalogPort, StoreCatalogService>();

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
        services.AddScoped<ICurrentUserContext, HttpCurrentUserContext>();
        services.AddScoped<ICustomerRegistrationRepository, CustomerRegistrationRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IVatTypeRepository, VatTypeRepository>();
        services.AddScoped<IStockOverviewRepository, StockOverviewRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<ILowStockReadRepository, LowStockReadRepository>();
        services.AddScoped<ILowStockAlertService, NullLowStockAlertService>();
        services.AddScoped<IStockAdjustmentRepository, StockAdjustmentRepository>();
        services.AddScoped<IStockTransferRepository, StockTransferRepository>();
        services.AddScoped<IStockOrderRepository, StockOrderRepository>();
        services.AddScoped<IStockPoReceiveRepository, StockPoReceiveRepository>();
        services.AddScoped<IAuditLogRepository, NullAuditLogRepository>();
        services.AddScoped<IAuditService, NullAuditService>();
        services.AddScoped<IStoreCustomerRepository, StoreCustomerRepository>();
        services.AddScoped<IStoreProfileRepository, StoreProfileRepository>();
        services.AddScoped<IStoreOrderRepository, StoreOrderRepository>();
        services.AddScoped<IStockMovementService, StockMovementService>();
        services.AddScoped<IOrderStockWorkflowService, Stock.OrderStockWorkflowService>();
        services.AddHostedService<Stock.ReservationExpirationService>();
        services.AddWebShopMollie(configuration);

        services.AddWebShopNotifications(configuration);

        return services;
    }
}
