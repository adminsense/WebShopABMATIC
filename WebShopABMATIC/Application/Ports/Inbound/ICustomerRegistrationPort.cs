using WebShopABMATIC.Application.Store.Registration;

namespace WebShopABMATIC.Application.Ports;

public interface ICustomerRegistrationPort
{
    Task<CustomerRegistrationResult> RegisterAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default);
}
