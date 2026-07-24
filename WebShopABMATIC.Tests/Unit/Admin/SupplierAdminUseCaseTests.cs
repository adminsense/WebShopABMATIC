using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Suppliers;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class SupplierAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<ISupplierRepository>();
        var filter = new SupplierListFilter();
        repo.GetSuppliersAsync(filter, Arg.Any<CancellationToken>()).Returns(AdminTestHelpers.EmptyPage<SupplierDto>());
        (await new SupplierAdminUseCase(repo).GetSuppliersAsync(filter)).TotalCount.Should().Be(0);
    }
}
