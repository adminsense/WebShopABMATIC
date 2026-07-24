using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class AuditLogAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IAuditLogRepository>();
        var filter = new AuditLogListFilter();
        repo.GetPagedAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<AuditLogListItemDto>());
        (await new AuditLogAdminUseCase(repo).GetAuditTrailAsync(filter)).TotalCount.Should().Be(0);
    }
}
