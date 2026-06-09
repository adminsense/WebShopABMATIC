using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ApplicationUserAccountAdminUseCase : IApplicationUserAccountAdminPort
{
    private readonly IApplicationUserAccountRepository _repository;

    public ApplicationUserAccountAdminUseCase(IApplicationUserAccountRepository repository) =>
        _repository = repository;

    public Task<PagedResult<ApplicationUserAccountDto>> GetAccountsAsync(
        ApplicationUserAccountListFilter filter,
        CancellationToken cancellationToken = default) =>
        _repository.GetAccountsAsync(filter, cancellationToken);

    public Task<ApplicationUserAccountEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default) =>
        _repository.GetForEditAsync(id, cancellationToken);

    public Task<ApplicationUserAccountSaveResult> SaveAsync(
        ApplicationUserAccountEditDto dto,
        CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(dto, cancellationToken);

    public Task<PasswordResetResult> ResetPasswordAsync(
        string userId,
        string? newPassword = null,
        CancellationToken cancellationToken = default) =>
        _repository.ResetPasswordAsync(userId, newPassword, cancellationToken);
}
