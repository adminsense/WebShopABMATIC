using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Admin;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Infrastructure.Media;
using WebShopABMATIC.Infrastructure.Persistence.Repositories;
using WebShopABMATIC.Infrastructure.Seeding;
using WebShopABMATIC.Infrastructure.Store;

namespace WebShopABMATIC.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddWebShopInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConnection = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");

        var domainConnection = configuration.GetConnectionString("connWebShopABMATIC")
            ?? identityConnection;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(identityConnection));

        services.AddDbContext<WebShopABMATICDbContext>(options =>
            options.UseSqlServer(domainConnection));

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
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IVatTypeRepository, VatTypeRepository>();

        services.AddHostedService<IdentitySeedHostedService>();

        return services;
    }
}
