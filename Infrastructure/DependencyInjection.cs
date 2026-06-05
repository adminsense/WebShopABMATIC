using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Admin;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Infrastructure.Media;
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

        services.AddScoped<IAdminDashboardPort, AdminDashboardService>();
        services.AddSingleton<IAdminHubPort, AdminHubRegistry>();
        services.AddScoped<IProductMediaPort, LocalProductMediaService>();
        services.AddScoped<IProductAdminPort, ProductAdminService>();
        services.AddScoped<IWebshopStructureAdminPort, WebshopStructureAdminService>();
        services.AddScoped<IWebshopProductStructureAdminPort, WebshopProductStructureAdminService>();
        services.AddScoped<IProductPriceAdminPort, ProductPriceAdminService>();
        services.AddScoped<IProductQuantityTierAdminPort, ProductQuantityTierAdminService>();
        services.AddScoped<IProductOptionAdminPort, ProductOptionAdminService>();
        services.AddScoped<IPriceListCategoryAdminPort, PriceListCategoryAdminService>();
        services.AddScoped<IManufacturerAdminPort, ManufacturerAdminService>();
        services.AddScoped<ISupplierAdminPort, SupplierAdminService>();
        services.AddScoped<ICustomerDeliveryAddressAdminPort, CustomerDeliveryAddressAdminService>();
        services.AddScoped<ICustomerProductDiscountAdminPort, CustomerProductDiscountAdminService>();
        services.AddScoped<ICustomerTypeAdminPort, CustomerTypeAdminService>();
        services.AddScoped<ICustomerAdminPort, CustomerAdminService>();
        services.AddScoped<IOrderStatusAdminPort, OrderStatusAdminService>();
        services.AddScoped<IDeliveryTypeAdminPort, DeliveryTypeAdminService>();
        services.AddScoped<IOrderAdminPort, OrderAdminService>();
        services.AddScoped<IProductStockLocationAdminPort, ProductStockLocationAdminService>();
        services.AddScoped<IStockLocationAdminPort, StockLocationAdminService>();
        services.AddScoped<IPaymentMethodAdminPort, PaymentMethodAdminService>();
        services.AddScoped<IStaffUserAdminPort, StaffUserAdminService>();
        services.AddScoped<IUserGroupAdminPort, UserGroupAdminService>();
        services.AddScoped<IVatTypeAdminPort, VatTypeAdminService>();
        services.AddScoped<IStoreCatalogPort, StoreCatalogService>();

        services.AddHostedService<IdentitySeedHostedService>();

        return services;
    }
}
