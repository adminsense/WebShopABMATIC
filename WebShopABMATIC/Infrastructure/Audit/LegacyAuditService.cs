using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class LegacyAuditService : IAuditService
{
    private readonly WebShopABMATICDbContext _db;
    private readonly LegacyAuditWriter _writer;
    private readonly ICurrentUserContext _currentUser;

    public LegacyAuditService(
        WebShopABMATICDbContext db,
        LegacyAuditWriter writer,
        ICurrentUserContext currentUser)
    {
        _db = db;
        _writer = writer;
        _currentUser = currentUser;
    }

    public async Task LogAsync(AuditLogWriteRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _currentUser.GetCurrentUserAsync(cancellationToken);
        var userName = LegacyAuditWriter.Truncate(
            !string.IsNullOrWhiteSpace(request.UserDisplayName)
                ? request.UserDisplayName!
                : user.AuditLabel,
            50);

        var module = ResolveModule(request.Action);
        var message = BuildMessage(request);
        var inner = BuildInner(request);

        await _writer.WriteAppErrorAsync(
            _db,
            module,
            message,
            inner,
            userName,
            LegacyAuditWriter.Truncate(request.EntityName, 50),
            saveChanges: false,
            cancellationToken);

        if (string.Equals(request.EntityName, "Order", StringComparison.OrdinalIgnoreCase)
            && int.TryParse(request.EntityId, out var orderId)
            && orderId > 0)
        {
            await _writer.WriteOrderLogAsync(
                _db,
                orderId,
                message,
                request.LegacyStaffUserId ?? user.StaffUserId,
                saveChanges: false,
                cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task LogExceptionAsync(
        Exception exception,
        string moduleName,
        string className,
        CancellationToken cancellationToken = default)
    {
        var user = await _currentUser.GetCurrentUserAsync(cancellationToken);
        await _writer.WriteAppErrorAsync(
            _db,
            moduleName,
            exception.Message,
            exception.InnerException?.Message,
            user.AuditLabel,
            className,
            saveChanges: true,
            cancellationToken);
    }

    private static string ResolveModule(string action) => action switch
    {
        AuditActions.Login or AuditActions.LoginFailed or AuditActions.Logout => LegacyAuditModules.Auth,
        AuditActions.CheckoutStarted => LegacyAuditModules.Checkout,
        AuditActions.PaymentPaid or AuditActions.PaymentExpired => LegacyAuditModules.Mollie,
        AuditActions.StockAdjust => LegacyAuditModules.Stock,
        AuditActions.ReportExport => LegacyAuditModules.Admin,
        AuditActions.OrderCancelled => LegacyAuditModules.Admin,
        AuditActions.PasswordReset => LegacyAuditModules.Admin,
        _ => LegacyAuditModules.Audit
    };

    private static string BuildMessage(AuditLogWriteRequest request)
    {
        var idPart = string.IsNullOrWhiteSpace(request.EntityId) ? "" : $" id={request.EntityId}";
        if (request.Success)
        {
            return LegacyAuditWriter.Truncate($"{request.Action} {request.EntityName}{idPart}", 1024);
        }

        var err = string.IsNullOrWhiteSpace(request.ErrorMessage) ? "failed" : request.ErrorMessage;
        return LegacyAuditWriter.Truncate($"Failed {request.Action} {request.EntityName}{idPart}: {err}", 1024);
    }

    private static string BuildInner(AuditLogWriteRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.AdditionalInfo))
        {
            return request.AdditionalInfo!;
        }

        if (!string.IsNullOrWhiteSpace(request.NewValues))
        {
            return request.NewValues!;
        }

        if (!string.IsNullOrWhiteSpace(request.OldValues))
        {
            return request.OldValues!;
        }

        if (!string.IsNullOrWhiteSpace(request.ErrorMessage) && request.Success)
        {
            return request.ErrorMessage!;
        }

        return string.Empty;
    }
}
