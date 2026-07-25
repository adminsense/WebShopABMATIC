using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace WebShopABMATIC.Tests.Integration.Sql;

public sealed class StaffUsersSqlReadTests
{
    [Fact]
    [Trait("Category", "SqlIntegration")]
    public async Task StaffUsers_table_readable()
    {
        if (!SqlIntegrationGate.HasConnection) return;

        await using var db = SqlIntegrationGate.CreateDb();
        var count = await db.StaffUsers.AsNoTracking().CountAsync();
        count.Should().BeGreaterThanOrEqualTo(0);
    }
}
