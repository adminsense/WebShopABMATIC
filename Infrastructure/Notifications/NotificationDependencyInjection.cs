using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Notifications;

public static class NotificationDependencyInjection
{
    public static IServiceCollection AddWebShopNotifications(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var useMock = configuration.GetValue(
            "Notifications:LowStock:UseMock",
            environment.IsDevelopment());

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
