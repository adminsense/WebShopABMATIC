using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Admin.UserGroups;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IUserGroupRepository
{
    Task<PagedResult<UserGroupDto>> GetUserGroupsAsync(UserGroupListFilter filter, CancellationToken cancellationToken = default);
    Task<UserGroupEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(UserGroupEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}