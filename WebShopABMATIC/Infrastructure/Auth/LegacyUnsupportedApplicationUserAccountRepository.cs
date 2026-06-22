using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class LegacyUnsupportedApplicationUserAccountRepository : IApplicationUserAccountRepository
{
    private const string Message = "Identity accounts are not available in legacy mode.";

    public Task<PagedResult<ApplicationUserAccountDto>> GetAccountsAsync(
        ApplicationUserAccountListFilter filter,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(new PagedResult<ApplicationUserAccountDto>
        {
            Items = [],
            TotalCount = 0,
            Page = filter.Page,
            PageSize = filter.PageSize
        });

    public Task<ApplicationUserAccountEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default) =>
        Task.FromResult<ApplicationUserAccountEditDto?>(null);

    public Task<ApplicationUserAccountSaveResult> SaveAsync(
        ApplicationUserAccountEditDto dto,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(new ApplicationUserAccountSaveResult { Succeeded = false, Errors = [Message] });

    public Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PasswordResetResult { Succeeded = false, Errors = [Message] });
}
