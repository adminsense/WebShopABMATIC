using WebShopABMATIC.Application.Admin.SystemUsers;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ISystemUserRepository
{
    Task<PagedResult<SystemUserDto>> GetSystemUsersAsync(SystemUserListFilter filter, CancellationToken cancellationToken = default);

    Task<SystemUserEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default);

    Task<SystemUserSaveResult> SaveAsync(SystemUserEditDto dto, CancellationToken cancellationToken = default);

    Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default);
}
