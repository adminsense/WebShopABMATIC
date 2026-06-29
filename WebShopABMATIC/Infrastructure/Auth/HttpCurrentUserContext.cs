using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    private CurrentUserSnapshot? _cached;

    public Task<CurrentUserSnapshot> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (_cached is not null)
        {
            return Task.FromResult(_cached);
        }

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            _cached = CurrentUserSnapshot.Anonymous;
            return Task.FromResult(_cached);
        }

        var principal = httpContext.User;
        int? staffUserId = ParseClaimInt(principal, LegacyAuthClaims.StaffUserId);
        int? customerId = ParseClaimInt(principal, LegacyAuthClaims.CustomerId);
        var displayName = principal.FindFirstValue(LegacyAuthClaims.DisplayName)
            ?? principal.Identity?.Name
            ?? "User";
        var login = principal.FindFirstValue(LegacyAuthClaims.Login)
            ?? principal.Identity?.Name
            ?? displayName;
        var auditLabel = TruncateAuditLabel(login);

        _cached = new CurrentUserSnapshot
        {
            IsAuthenticated = true,
            CustomerId = customerId,
            StaffUserId = staffUserId,
            DisplayName = displayName,
            AuditLabel = auditLabel
        };

        return Task.FromResult(_cached);
    }

    private static int? ParseClaimInt(ClaimsPrincipal principal, string claimType)
    {
        var value = principal.FindFirstValue(claimType);
        return int.TryParse(value, out var id) ? id : null;
    }

    private static string TruncateAuditLabel(string value)
    {
        var trimmed = value.Trim();
        return trimmed.Length <= 50 ? trimmed : trimmed[..50];
    }
}
