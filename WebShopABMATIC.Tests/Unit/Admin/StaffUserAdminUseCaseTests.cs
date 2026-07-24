using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class StaffUserAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IStaffUserRepository>();
        var filter = new StaffUserListFilter();
        repo.GetStaffUsersAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<StaffUserDto>());
        (await new StaffUserAdminUseCase(repo).GetStaffUsersAsync(filter)).TotalCount.Should().Be(0);
    }
}
