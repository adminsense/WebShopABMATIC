using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStaffUserRepository
{
    Task<PagedResult<StaffUserDto>> GetStaffUsersAsync(StaffUserListFilter filter, CancellationToken cancellationToken = default);
    Task<StaffUserEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StaffUserGroupLookupDto>> GetGroupLookupsAsync(CancellationToken cancellationToken = default);
    Task<int> SaveAsync(StaffUserEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
