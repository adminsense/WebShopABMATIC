using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class LegacyAuditWriter
{
    private static readonly HashSet<string> LogEntityNames =
    [
        nameof(OrderLog),
        nameof(AppError),
        nameof(ProjectActivity),
        nameof(StockMovement)
    ];

    private readonly ICurrentUserContext _currentUser;

    public LegacyAuditWriter(ICurrentUserContext currentUser) => _currentUser = currentUser;

    public async Task WriteOrderLogAsync(
        WebShopABMATICDbContext db,
        int orderId,
        string description,
        int? userIdOverride,
        bool saveChanges,
        CancellationToken cancellationToken)
    {
        var userId = userIdOverride ?? (await _currentUser.GetCurrentUserAsync(cancellationToken)).ResolveLegacyUserId();
        db.OrderLogs.Add(new OrderLog
        {
            OrderId = orderId,
            UserId = userId,
            Description = Truncate(description, 500)
        });

        if (saveChanges)
        {
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task WriteProjectActivityAsync(
        WebShopABMATICDbContext db,
        int projectId,
        int actionCode,
        bool saveChanges,
        CancellationToken cancellationToken)
    {
        if (projectId <= 0)
        {
            return;
        }

        db.ProjectActivities.Add(new ProjectActivity
        {
            ProjectId = projectId,
            ActionCode = actionCode,
            LoggedAt = DateTime.Now
        });

        if (saveChanges)
        {
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task WriteAppErrorAsync(
        WebShopABMATICDbContext db,
        string moduleName,
        string message,
        string? innerMessage,
        string userName,
        string className,
        bool saveChanges,
        CancellationToken cancellationToken)
    {
        db.AppErrors.Add(new AppError
        {
            DateTime = DateTime.Now,
            ModuleName = Truncate(moduleName, 50),
            Exception = Truncate(message, 1024),
            InnerExceptionMessage = Truncate(innerMessage ?? string.Empty, 1024),
            UserName = Truncate(userName, 50),
            ClassName = Truncate(className, 50)
        });

        if (saveChanges)
        {
            await db.SaveChangesAsync(cancellationToken);
        }
    }

    public void AttachCrudAppError(WebShopABMATICDbContext db, EntityEntry entry, string userName)
    {
        var entityName = entry.Entity.GetType().Name;
        if (LogEntityNames.Contains(entityName))
        {
            return;
        }

        var action = entry.State switch
        {
            EntityState.Added => AuditActions.Create,
            EntityState.Modified => AuditActions.Update,
            EntityState.Deleted => AuditActions.Delete,
            _ => null
        };

        if (action is null)
        {
            return;
        }

        var entityId = GetPrimaryKeyValue(entry);
        var message = $"{action} {entityName} id={entityId ?? "?"}";

        db.AppErrors.Add(new AppError
        {
            DateTime = DateTime.Now,
            ModuleName = Truncate(LegacyAuditModules.Audit, 50),
            Exception = Truncate(message, 1024),
            InnerExceptionMessage = string.Empty,
            UserName = Truncate(userName, 50),
            ClassName = Truncate(entityName, 50)
        });
    }

    public static bool IsLogEntityType(string entityTypeName) => LogEntityNames.Contains(entityTypeName);

    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Length <= maxLength ? value : value[..maxLength];
    }

    private static string? GetPrimaryKeyValue(EntityEntry entry)
    {
        var key = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
        return key?.CurrentValue?.ToString() ?? key?.OriginalValue?.ToString();
    }
}
