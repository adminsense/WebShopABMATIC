using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.StaffUsers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Persistence;

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

        var pageRows = await query
            .OrderBy(e => e.Login)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new
            {
                e.Id,
                e.Login,
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.UserGroupId,
                e.Tel,
                e.Admin,
                e.Bestellingen,
                e.Productie
            })
            .ToListAsync(cancellationToken);

        var groupIds = pageRows.Where(r => r.UserGroupId is > 0).Select(r => r.UserGroupId!.Value).Distinct().ToList();
        var groupNames = groupIds.Count == 0
            ? new Dictionary<int, string>()
            : await _db.UserGroups.AsNoTracking()
                .Where(g => groupIds.Contains(g.Id))
                .ToDictionaryAsync(g => g.Id, g => g.Name, cancellationToken);

        var items = pageRows.Select(e => new StaffUserDto
        {
            Id = e.Id,
            Login = e.Login,
            FirstName = e.FirstName,
            LastName = e.LastName,
            JobTitle = e.JobTitle,
            UserGroupId = e.UserGroupId,
            UserGroupName = e.UserGroupId is int gid && groupNames.TryGetValue(gid, out var name) ? name : null,
            Tel = e.Tel,
            IsAdmin = e.Admin,
            IsManager = e.Bestellingen || e.Productie || e.Admin
        }).ToList();

        return new PagedResult<StaffUserDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<StaffUserEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var e = await _db.StaffUsers.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Login,
                x.FirstName,
                x.LastName,
                x.JobTitle,
                x.UserGroupId,
                x.Tel,
                x.Admin,
                x.Bestellingen,
                x.Productie
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (e is null)
        {
            return null;
        }

        return new StaffUserEditDto
        {
            Id = e.Id,
            Login = e.Login,
            FirstName = e.FirstName,
            LastName = e.LastName,
            JobTitle = e.JobTitle,
            UserGroupId = e.UserGroupId,
            Tel = e.Tel,
            IsAdmin = e.Admin,
            IsManager = e.Bestellingen || e.Productie || e.Admin
            // Password intentionally omitted
        };
    }

    public async Task<IReadOnlyList<StaffUserGroupLookupDto>> GetGroupLookupsAsync(CancellationToken cancellationToken = default) =>
        await _db.UserGroups.AsNoTracking()
            .OrderBy(g => g.Name)
            .Select(g => new StaffUserGroupLookupDto { Id = g.Id, Name = g.Name })
            .ToListAsync(cancellationToken);

    public async Task<int> SaveAsync(StaffUserEditDto dto, CancellationToken cancellationToken = default)
    {
        StaffUser entity;
        var isCreate = dto.Id == 0;
        if (isCreate)
        {
            entity = (StaffUser)AdminCrudDefaults.Create("staff-users");
            _db.StaffUsers.Add(entity);
        }
        else
        {
            entity = await _db.StaffUsers.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Login = dto.Login.Trim();
        entity.FirstName = dto.FirstName.Trim();
        entity.LastName = dto.LastName.Trim();
        entity.JobTitle = string.IsNullOrWhiteSpace(dto.JobTitle) ? null : dto.JobTitle.Trim();
        entity.UserGroupId = dto.UserGroupId is > 0 ? dto.UserGroupId : null;
        entity.Tel = string.IsNullOrWhiteSpace(dto.Tel) ? null : dto.Tel.Trim();
        // Group XOR Admin/Manager — never persist both.
        if (entity.UserGroupId is > 0)
        {
            entity.Admin = false;
            entity.Bestellingen = false;
            entity.Productie = false;
        }
        else
        {
            entity.Admin = dto.IsAdmin;
            entity.Bestellingen = dto.IsManager;
            if (!dto.IsManager)
                entity.Productie = false;
        }

        var password = dto.Password?.Trim() ?? string.Empty;
        if (isCreate || password.Length > 0)
        {
            entity.Password = password;
        }

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
