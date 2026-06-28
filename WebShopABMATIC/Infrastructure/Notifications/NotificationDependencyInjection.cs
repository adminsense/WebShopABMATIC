using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Notifications;

public static class NotificationDependencyInjection
{
    public static IServiceCollection AddWebShopNotifications(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var useMock = configuration.GetValue("Notifications:LowStock:UseMock", true);

        if (useMock)
        {
            services.AddScoped<ILowStockEmailNotifier, MockLowStockEmailNotifier>();
        }
        else
        {
            services.AddScoped<ILowStockEmailNotifier, LowStockEmailNotifier>();
        }

        return services;
    }
}
