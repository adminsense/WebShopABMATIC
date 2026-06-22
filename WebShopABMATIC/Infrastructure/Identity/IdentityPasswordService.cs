using Microsoft.AspNetCore.Identity;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Audit;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class IdentityPasswordService : IIdentityPasswordPort
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _audit;

    public IdentityPasswordService(UserManager<ApplicationUser> userManager, IAuditService audit)
    {
        _userManager = userManager;
        _audit = audit;
    }

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

        await AuditManualLogger.LogIdentityUserAsync(
            _audit,
            AuditActions.PasswordReset,
            user.Id,
            user.Email,
            new { passwordReset = true, temporaryGenerated = string.IsNullOrWhiteSpace(newPassword) },
            cancellationToken);

        return new PasswordResetResult
        {
            Succeeded = true,
            TemporaryPassword = string.IsNullOrWhiteSpace(newPassword) ? password : null
        };
    }

    private static PasswordResetResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
