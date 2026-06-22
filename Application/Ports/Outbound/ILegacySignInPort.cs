using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ILegacySignInPort
{
    Task<LegacySignInResult> SignInStaffAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default);

    Task<LegacySignInResult> SignInCustomerAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default);
}
