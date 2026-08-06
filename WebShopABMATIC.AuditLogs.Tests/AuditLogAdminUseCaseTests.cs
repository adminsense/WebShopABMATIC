using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.AuditLogs.Tests;

public sealed class AuditLogAdminUseCaseTests
{
    [Fact]
    public async Task GetAuditTrailAsync_forwards_filter_to_repository()
    {
        var filter = new AuditLogListFilter
        {
            Action = AuditActions.Login,
            EntityName = LegacyAuditModules.Auth,
            Page = 2
        };
        var expected = new PagedResult<AuditLogListItemDto>
        {
            Items =
            [
                new AuditLogListItemDto
                {
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    Action = AuditActions.Login,
                    EntityName = LegacyAuditModules.Auth,
                    UserDisplayName = "admin",
                    Severity = AuditSeverity.Information,
                    Success = true
                }
            ],
            TotalCount = 1,
            Page = 2,
            PageSize = 25
        };

        var repo = new Mock<IAuditLogRepository>();
        repo.Setup(r => r.GetPagedAsync(filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var sut = new AuditLogAdminUseCase(repo.Object);

        var result = await sut.GetAuditTrailAsync(filter);

        result.Should().BeSameAs(expected);
        repo.Verify(r => r.GetPagedAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDetailAsync_returns_repository_result()
    {
        var detail = new AuditLogDetailDto
        {
            Id = 9,
            Timestamp = DateTime.UtcNow,
            Action = AuditActions.Update,
            EntityName = LegacyAuditModules.Audit,
            UserDisplayName = "manager",
            Severity = AuditSeverity.Information,
            Success = true,
            AdditionalInfo = "Update Product id=9"
        };

        var repo = new Mock<IAuditLogRepository>();
        repo.Setup(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var sut = new AuditLogAdminUseCase(repo.Object);

        var result = await sut.GetDetailAsync(9);

        result.Should().BeSameAs(detail);
        repo.Verify(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDetailAsync_returns_null_when_missing()
    {
        var repo = new Mock<IAuditLogRepository>();
        repo.Setup(r => r.GetByIdAsync(404, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuditLogDetailDto?)null);

        var sut = new AuditLogAdminUseCase(repo.Object);

        var result = await sut.GetDetailAsync(404);

        result.Should().BeNull();
    }
}
