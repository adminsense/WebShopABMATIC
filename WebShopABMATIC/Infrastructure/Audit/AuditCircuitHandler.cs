using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Audit;

public sealed class AuditCircuitHandler : CircuitHandler
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _identityUserId;
    private string? _email;

    public AuditCircuitHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor)
    {
        _scopeFactory = scopeFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        CaptureUserFromHttpContext();
        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override async Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_identityUserId))
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var tracker = scope.ServiceProvider.GetRequiredService<IManualLogoutTracker>();
            if (!tracker.WasManualLogoutRecently(_identityUserId))
            {
                var audit = scope.ServiceProvider.GetRequiredService<IAuditService>();
                await audit.LogAsync(new AuditLogWriteRequest
                {
                    Action = AuditActions.Logout,
                    EntityName = "ApplicationUser",
                    EntityId = _identityUserId,
                    IdentityUserId = _identityUserId,
                    UserDisplayName = _email,
                    NewValues = JsonSerializer.Serialize(new
                    {
                        email = _email,
                        reason = "CircuitClosed"
                    })
                }, cancellationToken);
            }
        }

        await base.OnCircuitClosedAsync(circuit, cancellationToken);
    }

    private void CaptureUserFromHttpContext()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return;
        }

        _identityUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        _email = user.FindFirstValue(ClaimTypes.Email) ?? user.Identity?.Name;
    }
}
