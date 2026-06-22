using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebShopABMATIC.Data.Persistence;

public sealed class WebShopABMATICDbContextFactory : IDesignTimeDbContextFactory<WebShopABMATICDbContext>
{
    public WebShopABMATICDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("connWebShopABMATIC")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=WebShopABMATIC_Dev;Trusted_Connection=True;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<WebShopABMATICDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new WebShopABMATICDbContext(optionsBuilder.Options);
    }
}
