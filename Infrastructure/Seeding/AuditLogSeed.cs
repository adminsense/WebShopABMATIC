using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebShopABMATIC.Infrastructure.Seeding;

public static class AuditLogSeed
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await db.AuditLogs.AsNoTracking().AnyAsync(cancellationToken))
        {
            return;
        }

        var now = DateTime.UtcNow;
        db.AuditLogs.AddRange(
            Row(now.AddMinutes(-45), AuditActions.Login, "ApplicationUser", "seed-admin", "admin@webshop.com", true,
                newValues: """{"email":"admin@webshop.com","roles":["Admin","Manager"]}"""),
            Row(now.AddMinutes(-40), AuditActions.Create, "Product", "101", "admin@webshop.com", true,
                newValues: """{"nameEn":"Demo pump","orderPartNumber":"PMP-101"}"""),
            Row(now.AddMinutes(-35), AuditActions.Update, "Product", "101", "admin@webshop.com", true,
                oldValues: """{"nameEn":"Demo pump","grossSalesPrice":120.00}""",
                newValues: """{"nameEn":"Demo pump XL","grossSalesPrice":135.50}"""),
            Row(now.AddMinutes(-30), AuditActions.ReportExport, "ProductsCatalogReport", null, "admin@webshop.com", true,
                newValues: """{"reportKey":"ProductsCatalogReport","format":"csv"}"""),
            Row(now.AddMinutes(-25), AuditActions.Create, "Customer", "42", "manager@webshop.com", true,
                newValues: """{"name":"ACME BV","email":"acme@example.com"}"""),
            Row(now.AddMinutes(-20), AuditActions.LoginFailed, "ApplicationUser", null, "unknown@test.com", false,
                severity: AuditSeverity.Warning, errorMessage: "Invalid email or password",
                newValues: """{"email":"unknown@test.com","reason":"InvalidPassword"}"""),
            Row(now.AddMinutes(-15), AuditActions.Update, "Order", "5001", "manager@webshop.com", true,
                oldValues: """{"statusId":2}""", newValues: """{"statusId":5}"""),
            Row(now.AddMinutes(-10), AuditActions.Delete, "ProductOption", "7", "admin@webshop.com", true,
                oldValues: """{"nameEn":"Color red"}"""),
            Row(now.AddMinutes(-5), AuditActions.Logout, "ApplicationUser", "seed-admin", "admin@webshop.com", true,
                newValues: """{"reason":"ManualLogout"}"""),
            Row(now.AddMinutes(-2), AuditActions.ReportExport, "StockMovementsReport", null, "admin@webshop.com", true,
                newValues: """{"reportKey":"StockMovementsReport","format":"pdf","filters":{"dateFrom":"2026-01-01"}}"""));

        await db.SaveChangesAsync(cancellationToken);
    }

    private static AuditLog Row(
        DateTime timestamp,
        string action,
        string entityName,
        string? entityId,
        string user,
        bool success,
        string severity = AuditSeverity.Information,
        string? errorMessage = null,
        string? oldValues = null,
        string? newValues = null) =>
        new()
        {
            Timestamp = timestamp,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            UserDisplayName = user,
            Severity = severity,
            Success = success,
            ErrorMessage = errorMessage,
            OldValues = oldValues,
            NewValues = newValues,
            IpAddress = "127.0.0.1",
            UserAgent = "Seed"
        };
}
