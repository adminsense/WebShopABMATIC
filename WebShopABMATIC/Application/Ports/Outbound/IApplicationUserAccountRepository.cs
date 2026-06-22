using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IApplicationUserAccountRepository
{
    Task<PagedResult<ApplicationUserAccountDto>> GetAccountsAsync(ApplicationUserAccountListFilter filter, CancellationToken cancellationToken = default);

    Task<ApplicationUserAccountEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default);

    Task<ApplicationUserAccountSaveResult> SaveAsync(ApplicationUserAccountEditDto dto, CancellationToken cancellationToken = default);

    Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default);
}
