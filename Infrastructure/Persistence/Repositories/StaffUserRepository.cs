using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StaffUserRepository : IStaffUserRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StaffUserRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<StaffUserDto>> GetStaffUsersAsync(StaffUserListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.StaffUsers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e =>
                e.Login.Contains(term) ||
                e.FirstName.Contains(term) ||
                e.LastName.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Login)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new StaffUserDto
            {
                Id = e.Id,
                Login = e.Login,
                FirstName = e.FirstName,
                LastName = e.LastName,
                JobTitle = e.JobTitle,
                UserGroupId = e.UserGroupId,
                Tel = e.Tel
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<StaffUserDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<StaffUserEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.StaffUsers.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new StaffUserEditDto
            {
                Id = e.Id,
                Login = e.Login,
                FirstName = e.FirstName,
                LastName = e.LastName,
                JobTitle = e.JobTitle,
                UserGroupId = e.UserGroupId,
                Tel = e.Tel
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(StaffUserEditDto dto, CancellationToken cancellationToken = default)
    {
        StaffUser entity;
        if (dto.Id == 0)
        {
            entity = (StaffUser)AdminCrudDefaults.Create("staff-users");
            _db.StaffUsers.Add(entity);
        }
        else
        {
            entity = await _db.StaffUsers.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Login = dto.Login;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.JobTitle = dto.JobTitle;
        entity.UserGroupId = dto.UserGroupId;
        entity.Tel = dto.Tel;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.StaffUsers.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.StaffUsers.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
