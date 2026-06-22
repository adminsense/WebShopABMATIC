using WebShopABMATIC.Application.Store.Registration;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ICustomerRegistrationRepository
{
    Task<CustomerRegistrationResult> RegisterAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default);
}
