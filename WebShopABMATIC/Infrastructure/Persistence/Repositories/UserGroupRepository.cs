using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.UserGroups;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class UserGroupRepository : IUserGroupRepository
{
    private readonly WebShopABMATICDbContext _db;

    public UserGroupRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<UserGroupDto>> GetUserGroupsAsync(UserGroupListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.UserGroups.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Name.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new UserGroupDto
            {
                Id = e.Id,
                Name = e.Name,
                IsInstallationTeam = e.IsInstallationTeam,
                IsServiceTeam = e.IsServiceTeam,
                IsTransportTeam = e.IsTransportTeam
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<UserGroupDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<UserGroupEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.UserGroups.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new UserGroupEditDto
            {
                Id = e.Id,
                Name = e.Name,
                IsInstallationTeam = e.IsInstallationTeam,
                IsServiceTeam = e.IsServiceTeam,
                IsTransportTeam = e.IsTransportTeam
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(UserGroupEditDto dto, CancellationToken cancellationToken = default)
    {
        UserGroup entity;
        if (dto.Id == 0)
        {
            entity = (UserGroup)AdminCrudDefaults.Create("user-groups");
            _db.UserGroups.Add(entity);
        }
        else
        {
            entity = await _db.UserGroups.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.IsInstallationTeam = dto.IsInstallationTeam;
        entity.IsServiceTeam = dto.IsServiceTeam;
        entity.IsTransportTeam = dto.IsTransportTeam;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.UserGroups.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.UserGroups.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
