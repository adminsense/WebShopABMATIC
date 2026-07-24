using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.VatTypes;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class VatTypeAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IVatTypeRepository>();
        var filter = new VatTypeListFilter();
        repo.GetVatTypesAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<VatTypeDto>());
        (await new VatTypeAdminUseCase(repo).GetVatTypesAsync(filter)).Items.Should().BeEmpty();
    }
}
