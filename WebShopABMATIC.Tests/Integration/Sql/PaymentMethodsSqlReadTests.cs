using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace WebShopABMATIC.Tests.Integration.Sql;

public sealed class PaymentMethodsSqlReadTests
{
    [Fact]
    [Trait("Category", "SqlIntegration")]
    public async Task PaymentMethods_table_readable()
    {
        if (!SqlIntegrationGate.HasConnection) return;

        await using var db = SqlIntegrationGate.CreateDb();
        var count = await db.PaymentMethods.AsNoTracking().CountAsync();
        count.Should().BeGreaterThanOrEqualTo(0);
    }
}
