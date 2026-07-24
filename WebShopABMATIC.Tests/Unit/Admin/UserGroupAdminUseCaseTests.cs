using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.UserGroups;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class UserGroupAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IUserGroupRepository>();
        var filter = new UserGroupListFilter();
        repo.GetUserGroupsAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<UserGroupDto>());
        (await new UserGroupAdminUseCase(repo).GetUserGroupsAsync(filter)).Page.Should().Be(1);
    }
}
