using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IAuditSuppressionContext _suppression;
    private readonly List<AuditLogWriteRequest> _pending = new();

    public AuditSaveChangesInterceptor(
        IServiceScopeFactory scopeFactory,
        IAuditSuppressionContext suppression)
    {
        _scopeFactory = scopeFactory;
        _suppression = suppression;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        _pending.Clear();
        if (eventData.Context is not null)
        {
            CapturePending(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var saved = await base.SavedChangesAsync(eventData, result, cancellationToken);

        if (saved > 0 && _pending.Count > 0)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var audit = scope.ServiceProvider.GetRequiredService<IAuditService>();
            foreach (var request in _pending)
            {
                await audit.LogAsync(request, cancellationToken);
            }
        }

        _pending.Clear();
        return saved;
    }

    private void CapturePending(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
            {
                continue;
            }

            var entityName = entry.Entity.GetType().Name;
            if (_suppression.IsSuppressed(entityName))
            {
                continue;
            }

            var entityId = AuditEntitySnapshot.ResolveEntityId(entry);

            switch (entry.State)
            {
                case EntityState.Added:
                    _pending.Add(new AuditLogWriteRequest
                    {
                        Action = AuditActions.Create,
                        EntityName = entityName,
                        EntityId = string.IsNullOrEmpty(entityId) ? null : entityId,
                        NewValues = AuditEntitySnapshot.SerializeCurrent(entry)
                    });
                    break;

                case EntityState.Deleted:
                    _pending.Add(new AuditLogWriteRequest
                    {
                        Action = AuditActions.Delete,
                        EntityName = entityName,
                        EntityId = string.IsNullOrEmpty(entityId) ? null : entityId,
                        OldValues = AuditEntitySnapshot.SerializeOriginal(entry)
                    });
                    break;

                case EntityState.Modified:
                {
                    var action = AuditEntitySnapshot.IsProductSoftDelete(entry)
                        ? AuditActions.Delete
                        : AuditActions.Update;

                    _pending.Add(new AuditLogWriteRequest
                    {
                        Action = action,
                        EntityName = entityName,
                        EntityId = string.IsNullOrEmpty(entityId) ? null : entityId,
                        OldValues = AuditEntitySnapshot.SerializeModifiedOriginal(entry)
                            ?? AuditEntitySnapshot.SerializeOriginal(entry),
                        NewValues = AuditEntitySnapshot.SerializeModified(entry)
                            ?? AuditEntitySnapshot.SerializeCurrent(entry)
                    });
                    break;
                }
            }
        }
    }
}
