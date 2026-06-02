using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Admin;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Infrastructure.Seeding;

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
        services.AddScoped<IProductAdminPort, ProductAdminService>();
        services.AddScoped<ICustomerAdminPort, CustomerAdminService>();
        services.AddScoped<IOrderAdminPort, OrderAdminService>();

        services.AddHostedService<IdentitySeedHostedService>();

        return services;
    }
}
