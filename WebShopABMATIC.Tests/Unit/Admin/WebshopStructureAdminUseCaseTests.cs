using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.WebshopStructures;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class WebshopStructureAdminUseCaseTests
{
    [Fact]
    public async Task List_delegates_to_repository()
    {
        var repo = Substitute.For<IWebshopStructureRepository>();
        var filter = new WebshopStructureListFilter();
        repo.GetWebshopStructuresAsync(filter, Arg.Any<CancellationToken>())
            .Returns(AdminTestHelpers.EmptyPage<WebshopStructureDto>());
        (await new WebshopStructureAdminUseCase(repo).GetWebshopStructuresAsync(filter)).TotalCount.Should().Be(0);
    }
}
