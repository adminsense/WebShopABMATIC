using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebShopABMATIC.Data.Persistence;

public sealed class WebShopABMATICDbContextFactory : IDesignTimeDbContextFactory<WebShopABMATICDbContext>
{
    public WebShopABMATICDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebShopABMATICDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=MULLER;Database=WebShopABMATIC;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        return new WebShopABMATICDbContext(optionsBuilder.Options);
    }
}
