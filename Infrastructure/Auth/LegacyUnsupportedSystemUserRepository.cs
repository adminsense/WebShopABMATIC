using WebShopABMATIC.Application.Admin.SystemUsers;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class LegacyUnsupportedSystemUserRepository : ISystemUserRepository
{
    private const string Message = "ASP.NET Identity users are not available in legacy mode.";

    public Task<PagedResult<SystemUserDto>> GetSystemUsersAsync(SystemUserListFilter filter, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PagedResult<SystemUserDto>
        {
            Items = [],
            TotalCount = 0,
            Page = filter.Page,
            PageSize = filter.PageSize
        });

    public Task<SystemUserEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default) =>
        Task.FromResult<SystemUserEditDto?>(null);

    public Task<SystemUserSaveResult> SaveAsync(SystemUserEditDto dto, CancellationToken cancellationToken = default) =>
        Task.FromResult(new SystemUserSaveResult { Succeeded = false, Errors = [Message] });

    public Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PasswordResetResult { Succeeded = false, Errors = [Message] });
}
