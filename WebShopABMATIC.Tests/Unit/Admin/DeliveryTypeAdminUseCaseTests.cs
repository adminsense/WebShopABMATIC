using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.DeliveryTypes;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class DeliveryTypeAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IDeliveryTypeRepository>();
        var filter = new DeliveryTypeListFilter();
        repo.GetDeliveryTypesAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<DeliveryTypeDto>());
        (await new DeliveryTypeAdminUseCase(repo).GetDeliveryTypesAsync(filter)).TotalCount.Should().Be(0);
        await repo.Received(1).GetDeliveryTypesAsync(filter, Arg.Any<CancellationToken>());
    }
}
