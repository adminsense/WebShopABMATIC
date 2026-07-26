using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Profile;
using WebShopABMATIC.Application.UseCases.Store;

namespace WebShopABMATIC.Tests.Unit.Store;

public sealed class StoreProfileUseCaseTests
{
    [Fact]
    public async Task GetMyProfile_returns_null_when_anonymous()
    {
        var current = Substitute.For<ICurrentUserContext>();
        current.GetCurrentUserAsync(Arg.Any<CancellationToken>()).Returns(CurrentUserSnapshot.Anonymous);
        var repo = Substitute.For<IStoreProfileRepository>();

        var profile = await new StoreProfileUseCase(repo, current).GetMyProfileAsync();

        profile.Should().BeNull();
        await repo.DidNotReceiveWithAnyArgs().GetByCustomerIdAsync(default, default);
    }

    [Fact]
    public async Task GetMyProfile_loads_for_customer()
    {
        var current = Substitute.For<ICurrentUserContext>();
        current.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot { IsAuthenticated = true, CustomerId = 5 });
        var repo = Substitute.For<IStoreProfileRepository>();
        repo.GetByCustomerIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(new StoreProfileDto { CustomerId = 5, FirstName = "Ada", LastName = "Lovelace" });

        var profile = await new StoreProfileUseCase(repo, current).GetMyProfileAsync();

        profile!.FirstName.Should().Be("Ada");
    }

    [Fact]
    public async Task SaveMyProfile_rejects_anonymous()
    {
        var current = Substitute.For<ICurrentUserContext>();
        current.GetCurrentUserAsync(Arg.Any<CancellationToken>()).Returns(CurrentUserSnapshot.Anonymous);

        var result = await new StoreProfileUseCase(Substitute.For<IStoreProfileRepository>(), current)
            .SaveMyProfileAsync(new StoreProfileUpdateDto
            {
                FirstName = "A",
                LastName = "B",
                Street = "S",
                HouseNumber = "1",
                PostalCode = "1000",
                CityName = "Brussels"
            });

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("signed in", StringComparison.OrdinalIgnoreCase));
    }
}
