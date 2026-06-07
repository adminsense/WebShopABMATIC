using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class HttpCurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly WebShopABMATICDbContext _domainDb;
    private CurrentUserSnapshot? _cached;

    public HttpCurrentUserContext(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        WebShopABMATICDbContext domainDb)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _domainDb = domainDb;
    }

    public async Task<CurrentUserSnapshot> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (_cached is not null)
        {
            return _cached;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            _cached = CurrentUserSnapshot.Anonymous;
            return _cached;
        }

        var identityUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(identityUserId))
        {
            _cached = CurrentUserSnapshot.Anonymous;
            return _cached;
        }

        var user = await _userManager.FindByIdAsync(identityUserId);
        if (user is null)
        {
            _cached = CurrentUserSnapshot.Anonymous;
            return _cached;
        }

        int? staffUserId = null;
        var email = user.Email ?? user.UserName;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalized = email.Trim().ToLowerInvariant();
            staffUserId = await _domainDb.StaffUsers.AsNoTracking()
                .Where(s => s.Login.ToLower() == normalized)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        var displayName = user.DisplayName;
        var auditLabel = TruncateAuditLabel(email ?? displayName);

        _cached = new CurrentUserSnapshot
        {
            IsAuthenticated = true,
            IdentityUserId = identityUserId,
            CustomerId = user.CustomerId,
            StaffUserId = staffUserId,
            DisplayName = displayName,
            AuditLabel = auditLabel
        };

        return _cached;
    }

    private static string TruncateAuditLabel(string value)
    {
        var trimmed = value.Trim();
        return trimmed.Length <= 50 ? trimmed : trimmed[..50];
    }
}
