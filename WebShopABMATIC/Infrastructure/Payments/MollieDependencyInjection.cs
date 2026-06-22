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
        var apiKey = configuration["Mollie:ApiKey"];
        if (!string.IsNullOrWhiteSpace(apiKey))
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
            services.AddScoped<IMolliePaymentPort, MolliePaymentNotConfiguredAdapter>();
        }

        return services;
    }

    private sealed class MolliePaymentNotConfiguredAdapter : IMolliePaymentPort
    {
        public Task<MolliePaymentCreated> CreatePaymentAsync(CreateMolliePaymentCommand command, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("Mollie is not configured. Set Mollie:ApiKey in user secrets or appsettings.");

        public Task<MolliePaymentStatusResult> GetPaymentAsync(string molliePaymentId, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("Mollie is not configured. Set Mollie:ApiKey in user secrets or appsettings.");
    }
}
