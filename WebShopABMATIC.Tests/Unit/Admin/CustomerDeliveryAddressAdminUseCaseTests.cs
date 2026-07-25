using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class CustomerDeliveryAddressAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<ICustomerDeliveryAddressRepository>();
        var filter = new CustomerDeliveryAddressListFilter();
        repo.GetCustomerDeliveryAddressesAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<CustomerDeliveryAddressDto>());
        (await new CustomerDeliveryAddressAdminUseCase(repo).GetCustomerDeliveryAddressesAsync(filter))
            .TotalCount.Should().Be(0);
    }
}
