using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.PaymentMethods;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class PaymentMethodAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IPaymentMethodRepository>();
        var filter = new PaymentMethodListFilter();
        repo.GetPaymentMethodsAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<PaymentMethodDto>());
        (await new PaymentMethodAdminUseCase(repo).GetPaymentMethodsAsync(filter)).TotalCount.Should().Be(0);
    }
}
