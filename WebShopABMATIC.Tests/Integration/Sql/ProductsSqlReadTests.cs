using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace WebShopABMATIC.Tests.Integration.Sql;

public sealed class ProductsSqlReadTests
{
    [Fact]
    [Trait("Category", "SqlIntegration")]
    public async Task Products_ShowOnWebshop_query_runs()
    {
        if (!SqlIntegrationGate.HasConnection) return;

        await using var db = SqlIntegrationGate.CreateDb();
        var count = await db.Products.AsNoTracking()
            .CountAsync(p => p.ShowOnWebshop == true && !p.IsInactive);
        count.Should().BeGreaterThanOrEqualTo(0);
    }
}
