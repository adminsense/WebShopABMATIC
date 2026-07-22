using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Profile;
using WebShopABMATIC.Application.Store.Registration;
using WebShopABMATIC.Application.UseCases.Store;

namespace WebShopABMATIC.Tests.Unit.Store;

public sealed class CustomerRegistrationUseCaseTests
{
    [Fact]
    public async Task RegisterAsync_delegates_to_repository()
    {
        var repo = Substitute.For<ICustomerRegistrationRepository>();
        var request = new CustomerRegistrationRequest
        {
            Email = "a@test.com",
            Password = "Secret1!",
            FirstName = "Ada",
            LastName = "Lovelace",
            Phone = "123",
            Street = "Main",
            HouseNumber = "1",
            PostalCode = "1000",
            CityName = "Brussels"
        };
        repo.RegisterAsync(request, Arg.Any<CancellationToken>())
            .Returns(new CustomerRegistrationResult { Succeeded = true, CustomerId = 99 });

        var result = await new CustomerRegistrationUseCase(repo).RegisterAsync(request);

        result.Succeeded.Should().BeTrue();
        result.CustomerId.Should().Be(99);
        await repo.Received(1).RegisterAsync(request, Arg.Any<CancellationToken>());
    }
}

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
