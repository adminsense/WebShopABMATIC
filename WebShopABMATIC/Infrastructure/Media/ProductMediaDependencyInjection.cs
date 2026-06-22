using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Media;

public static class ProductMediaDependencyInjection
{
    public static IServiceCollection AddProductMedia(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<AzureStorageOptions>(configuration.GetSection(AzureStorageOptions.SectionName));

        var section = configuration.GetSection(AzureStorageOptions.SectionName);
        var connectionString = section["ConnectionString"];
        var containerName = section["ContainerName"];

        if (!string.IsNullOrWhiteSpace(connectionString)
            && !string.IsNullOrWhiteSpace(containerName))
        {
            services.AddSingleton(_ => new BlobServiceClient(connectionString));
            services.AddScoped<IProductMediaPort, AzureBlobProductMediaService>();
        }
        else
        {
            services.AddScoped<IProductMediaPort, LocalProductMediaService>();
        }

        return services;
    }
}
