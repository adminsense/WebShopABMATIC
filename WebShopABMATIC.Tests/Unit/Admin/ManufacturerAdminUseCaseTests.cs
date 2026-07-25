using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Manufacturers;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ManufacturerAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IManufacturerRepository>();
        var filter = new ManufacturerListFilter();
        repo.GetManufacturersAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<ManufacturerDto>());
        (await new ManufacturerAdminUseCase(repo).GetManufacturersAsync(filter)).TotalCount.Should().Be(0);
    }
}
