using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.CustomerTypes;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class CustomerTypeAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<ICustomerTypeRepository>();
        var filter = new CustomerTypeListFilter();
        repo.GetCustomerTypesAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<CustomerTypeDto>());
        (await new CustomerTypeAdminUseCase(repo).GetCustomerTypesAsync(filter)).TotalCount.Should().Be(0);
    }
}
