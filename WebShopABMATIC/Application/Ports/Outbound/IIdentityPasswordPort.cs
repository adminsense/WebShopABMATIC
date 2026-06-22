using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IIdentityPasswordPort
{
    Task<PasswordResetResult> ResetPasswordAsync(string identityUserId, string? newPassword = null, CancellationToken cancellationToken = default);
}
