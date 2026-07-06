using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class LegacyAuditLogRepository : IAuditLogRepository
{
    private readonly WebShopABMATICDbContext _db;

    public LegacyAuditLogRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<AuditLogListItemDto>> GetPagedAsync(
        AuditLogListFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _db.AppErrors.AsNoTracking();

        if (filter.FromDate.HasValue)
        {
            query = query.Where(e => e.DateTime >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(e => e.DateTime <= filter.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Action))
        {
            query = query.Where(e => e.Exception.Contains(filter.Action) || e.ClassName.Contains(filter.Action));
        }

        if (!string.IsNullOrWhiteSpace(filter.EntityName))
        {
            query = query.Where(e => e.ModuleName.Contains(filter.EntityName) || e.ClassName.Contains(filter.EntityName));
        }

        if (!string.IsNullOrWhiteSpace(filter.UserSearch))
        {
            query = query.Where(e => e.UserName.Contains(filter.UserSearch));
        }

        if (!string.IsNullOrWhiteSpace(filter.Severity) && filter.Severity == AuditSeverity.Error)
        {
            query = query.Where(e => e.Exception.Contains("failed") || e.Exception.Contains("Failed"));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 200);

        var items = await query
            .OrderByDescending(e => e.DateTime)
            .ThenByDescending(e => e.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new AuditLogListItemDto
            {
                Id = e.Id,
                Timestamp = e.DateTime,
                Action = e.ClassName,
                EntityName = e.ModuleName,
                EntityId = null,
                UserDisplayName = e.UserName,
                Severity = e.ModuleName == LegacyAuditModules.Auth ? AuditSeverity.Information : AuditSeverity.Information,
                Success = !e.Exception.Contains("failed") && !e.Exception.Contains("Failed")
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<AuditLogListItemDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<AuditLogDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.AppErrors.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        return new AuditLogDetailDto
        {
            Id = entity.Id,
            Timestamp = entity.DateTime,
            Action = entity.ClassName,
            EntityName = entity.ModuleName,
            UserDisplayName = entity.UserName,
            Severity = AuditSeverity.Information,
            Success = !entity.Exception.Contains("failed") && !entity.Exception.Contains("Failed"),
            ErrorMessage = entity.InnerExceptionMessage,
            AdditionalInfo = entity.Exception,
            IpAddress = null,
            UserAgent = null
        };
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        _db.AppErrors.AsNoTracking().AnyAsync(cancellationToken);

    public async Task<IReadOnlyList<OrderLogListItemDto>> GetOrderLogsAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var logs = await (
            from log in _db.OrderLogs.AsNoTracking()
            where log.OrderId == orderId
            join staff in _db.StaffUsers.AsNoTracking() on log.UserId equals staff.Id into staffJoin
            from staff in staffJoin.DefaultIfEmpty()
            orderby log.Id descending
            select new OrderLogListItemDto
            {
                Id = log.Id,
                OrderId = log.OrderId,
                UserId = log.UserId,
                Description = log.Description,
                UserDisplayName = staff != null
                    ? (staff.FirstName + " " + staff.LastName).Trim()
                    : null
            }).ToListAsync(cancellationToken);

        return logs;
    }
}
