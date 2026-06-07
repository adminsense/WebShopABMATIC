using Microsoft.AspNetCore.Identity;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class IdentityPasswordService : IIdentityPasswordPort
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityPasswordService(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<PasswordResetResult> ResetPasswordAsync(string identityUserId, string? newPassword = null, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(identityUserId);
        if (user is null)
        {
            return Fail("User not found.");
        }

        var password = string.IsNullOrWhiteSpace(newPassword)
            ? IdentityPasswordGenerator.GenerateTemporaryPassword()
            : newPassword;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
        {
            return Fail(result.Errors.Select(e => e.Description).ToArray());
        }

        return new PasswordResetResult
        {
            Succeeded = true,
            TemporaryPassword = string.IsNullOrWhiteSpace(newPassword) ? password : null
        };
    }

    private static PasswordResetResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
