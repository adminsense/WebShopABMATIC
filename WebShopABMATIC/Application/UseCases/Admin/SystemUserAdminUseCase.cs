using WebShopABMATIC.Application.Admin.SystemUsers;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class SystemUserAdminUseCase : ISystemUserAdminPort
{
    private readonly ISystemUserRepository _repository;

    public SystemUserAdminUseCase(ISystemUserRepository repository) => _repository = repository;

    public Task<PagedResult<SystemUserDto>> GetSystemUsersAsync(SystemUserListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetSystemUsersAsync(filter, cancellationToken);

    public Task<SystemUserEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default) =>
        _repository.GetForEditAsync(id, cancellationToken);

    public Task<SystemUserSaveResult> SaveAsync(SystemUserEditDto dto, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(dto, cancellationToken);

    public Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default) =>
        _repository.ResetPasswordAsync(userId, newPassword, cancellationToken);
}