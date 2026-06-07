using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditLogRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PagedResult<AuditLogListItemDto>> GetPagedAsync(AuditLogListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.AuditLogs.AsNoTracking().AsQueryable();

        if (filter.FromDate.HasValue)
        {
            var from = filter.FromDate.Value.Date;
            query = query.Where(x => x.Timestamp >= from);
        }

        if (filter.ToDate.HasValue)
        {
            var to = filter.ToDate.Value.Date.AddDays(1);
            query = query.Where(x => x.Timestamp < to);
        }

        if (!string.IsNullOrWhiteSpace(filter.Action))
        {
            query = query.Where(x => x.Action == filter.Action);
        }

        if (!string.IsNullOrWhiteSpace(filter.Severity))
        {
            query = query.Where(x => x.Severity == filter.Severity);
        }

        if (!string.IsNullOrWhiteSpace(filter.EntityName))
        {
            query = query.Where(x => x.EntityName == filter.EntityName);
        }

        if (!string.IsNullOrWhiteSpace(filter.UserSearch))
        {
            var term = filter.UserSearch.Trim();
            query = query.Where(x => x.UserDisplayName.Contains(term));
        }

        if (filter.SuccessOnly == true)
        {
            query = query.Where(x => x.Success);
        }
        else if (filter.FailuresOnly == true)
        {
            query = query.Where(x => !x.Success);
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderByDescending(x => x.Timestamp)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AuditLogListItemDto
            {
                Id = x.Id,
                Timestamp = x.Timestamp,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                UserDisplayName = x.UserDisplayName,
                Severity = x.Severity,
                Success = x.Success
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
        return await _db.AuditLogs.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AuditLogDetailDto
            {
                Id = x.Id,
                Timestamp = x.Timestamp,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                IdentityUserId = x.IdentityUserId,
                LegacyStaffUserId = x.LegacyStaffUserId,
                UserDisplayName = x.UserDisplayName,
                Severity = x.Severity,
                Success = x.Success,
                ErrorMessage = x.ErrorMessage,
                IpAddress = x.IpAddress,
                UserAgent = x.UserAgent,
                OldValues = x.OldValues,
                NewValues = x.NewValues,
                DurationMs = x.DurationMs,
                AdditionalInfo = x.AdditionalInfo
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        _db.AuditLogs.AsNoTracking().AnyAsync(cancellationToken);

    public async Task AddAsync(AuditLogWriteRequest request, CancellationToken cancellationToken = default)
    {
        var http = _httpContextAccessor.HttpContext;
        var entity = new AuditLog
        {
            Timestamp = DateTime.UtcNow,
            Action = request.Action,
            EntityName = request.EntityName,
            EntityId = request.EntityId,
            IdentityUserId = request.IdentityUserId,
            LegacyStaffUserId = request.LegacyStaffUserId,
            UserDisplayName = request.UserDisplayName ?? "system",
            Severity = request.Severity,
            Success = request.Success,
            ErrorMessage = request.ErrorMessage,
            IpAddress = http?.Connection.RemoteIpAddress?.ToString(),
            UserAgent = Truncate(http?.Request.Headers.UserAgent.ToString(), 512),
            OldValues = request.OldValues,
            NewValues = request.NewValues,
            DurationMs = request.DurationMs,
            AdditionalInfo = request.AdditionalInfo
        };

        _db.AuditLogs.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value;
        }

        return value[..maxLength];
    }
}
