using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Ports.Outbound;
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
