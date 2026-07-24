using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class CustomerAdminUseCaseTests
{
    [Fact]
    public async Task List_and_password_reset_delegate()
    {
        var repo = Substitute.For<ICustomerRepository>();
        var filter = new CustomerListFilter();
        repo.GetCustomersAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<CustomerDto>());
        repo.ResetWebshopPasswordAsync(9, "new", Arg.Any<CancellationToken>())
            .Returns(new PasswordResetResult { Succeeded = true });

        var sut = new CustomerAdminUseCase(repo);
        (await sut.GetCustomersAsync(filter)).TotalCount.Should().Be(0);
        (await sut.ResetWebshopPasswordAsync(9, "new")).Succeeded.Should().BeTrue();
    }
}
