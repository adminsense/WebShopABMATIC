using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Registration;

namespace WebShopABMATIC.Application.UseCases.Store;

public sealed class CustomerRegistrationUseCase : ICustomerRegistrationPort
{
    private readonly ICustomerRegistrationRepository _repository;

    public CustomerRegistrationUseCase(ICustomerRegistrationRepository repository) => _repository = repository;

    public Task<CustomerRegistrationResult> RegisterAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default) =>
        _repository.RegisterAsync(request, cancellationToken);
}
