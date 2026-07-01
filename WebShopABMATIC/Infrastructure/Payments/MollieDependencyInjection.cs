using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api;
using Mollie.Api.Framework;
using WebShopABMATIC.Application.Payments;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Payments;

public static class MollieDependencyInjection
{
    public static IServiceCollection AddWebShopMollie(this IServiceCollection services, IConfiguration configuration)
    {
        var useMock = configuration.GetValue("Mollie:UseMock", true);
        var apiKey = configuration["Mollie:ApiKey"];

        if (!useMock && !string.IsNullOrWhiteSpace(apiKey))
        {
            services.AddMollieApi(options =>
            {
                options.ApiKey = apiKey;
                options.RetryPolicy = MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy();
            });
            services.AddScoped<IMolliePaymentPort, MolliePaymentAdapter>();
        }
        else
        {
            services.AddScoped<IMolliePaymentPort, MollieMockPaymentAdapter>();
        }

        return services;
    }
}
