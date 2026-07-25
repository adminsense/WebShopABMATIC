using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class CustomerProductDiscountAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<ICustomerProductDiscountRepository>();
        var filter = new CustomerProductDiscountListFilter();
        repo.GetCustomerProductDiscountsAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<CustomerProductDiscountDto>());
        (await new CustomerProductDiscountAdminUseCase(repo).GetCustomerProductDiscountsAsync(filter))
            .TotalCount.Should().Be(0);
    }
}
