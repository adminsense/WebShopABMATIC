using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Tests.Integration.Sql;

internal static class SqlIntegrationGate
{
    public static string? ConnectionString =>
        Environment.GetEnvironmentVariable("TEST_SQL_CONNECTION")
        ?? Environment.GetEnvironmentVariable("ConnectionStrings__connWebShopABMATIC");

    public static bool HasConnection => !string.IsNullOrWhiteSpace(ConnectionString);

    public static WebShopABMATICDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<WebShopABMATICDbContext>()
            .UseSqlServer(ConnectionString, sql => sql.CommandTimeout(30))
            .Options;
        return new WebShopABMATICDbContext(options);
    }
}
