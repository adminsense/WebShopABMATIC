using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Tests.Integration.Api;

public sealed class WebShopApiFactory : WebApplicationFactory<Program>
{
    public ILegacySignInPort SignIn { get; } = Substitute.For<ILegacySignInPort>();
    public IMollieWebhookPort MollieWebhook { get; } = Substitute.For<IMollieWebhookPort>();
    public IStockAdjustmentPort StockAdjustment { get; } = Substitute.For<IStockAdjustmentPort>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:connWebShopABMATIC"] =
                    "Server=(localdb)\\mssqllocaldb;Database=WebShopABMATIC_ApiTests_Unused;Trusted_Connection=True;TrustServerCertificate=True",
                ["Mollie:UseMock"] = "true",
                ["AzureBlob:ConnectionString"] = "",
                ["AzureBlob:ContainerName"] = "test"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            foreach (var hosted in services.Where(d => d.ServiceType == typeof(IHostedService)).ToList())
            {
                services.Remove(hosted);
            }

            services.RemoveAll<DbContextOptions<WebShopABMATICDbContext>>();
            services.RemoveAll<WebShopABMATICDbContext>();
            services.AddDbContext<WebShopABMATICDbContext>(options =>
                options.UseInMemoryDatabase("api-tests-" + Guid.NewGuid().ToString("N")));

            services.RemoveAll<ILegacySignInPort>();
            services.AddSingleton(SignIn);

            services.RemoveAll<IMollieWebhookPort>();
            services.AddSingleton(MollieWebhook);

            services.RemoveAll<IStockAdjustmentPort>();
            services.AddSingleton(StockAdjustment);
        });
    }
}
