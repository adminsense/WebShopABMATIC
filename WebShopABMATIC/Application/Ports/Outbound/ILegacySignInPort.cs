using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ILegacySignInPort
{
    /// <summary>
    /// Unified login: resolves against legacy <c>[Instellingen].[User]</c> first, then <c>Customers</c> webshop credentials.
    /// </summary>
    Task<LegacySignInResult> SignInAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default);

    Task<LegacySignInResult> SignInStaffAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default);

    Task<LegacySignInResult> SignInCustomerAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default);
}
