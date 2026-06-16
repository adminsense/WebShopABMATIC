using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class LegacySignInResult
{
    public bool Succeeded { get; init; }
    public string? Error { get; init; }
    public ClaimsPrincipal? Principal { get; init; }
}

public sealed class LegacySignInService
{
    private readonly WebShopABMATICDbContext _db;

    public LegacySignInService(WebShopABMATICDbContext db) => _db = db;

    public async Task<LegacySignInResult> SignInStaffAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default)
    {
        var normalized = loginOrEmail.Trim();
        var lower = normalized.ToLowerInvariant();

        var staff = await _db.StaffUsers.AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Login == normalized ||
                (u.EmailTemplate != null && u.EmailTemplate.ToLower() == lower), cancellationToken);

        if (staff is null || !string.Equals(staff.Password, password, StringComparison.Ordinal))
        {
            return new LegacySignInResult { Succeeded = false, Error = "Invalid login or password." };
        }

        var roles = new List<string>();
        if (staff.Admin)
        {
            roles.Add(AppRoles.Admin);
        }

        if (staff.Bestellingen || staff.Productie || staff.Admin)
        {
            roles.Add(AppRoles.Manager);
        }

        if (roles.Count == 0)
        {
            return new LegacySignInResult { Succeeded = false, Error = "This account is not authorized for the admin panel." };
        }

        var displayName = $"{staff.FirstName} {staff.LastName}".Trim();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, $"staff:{staff.Id}"),
            new(ClaimTypes.Name, staff.Login),
            new(LegacyAuthClaims.StaffUserId, staff.Id.ToString()),
            new(LegacyAuthClaims.DisplayName, displayName),
            new(LegacyAuthClaims.Login, staff.Login)
        };
        claims.AddRange(roles.Distinct().Select(r => new Claim(ClaimTypes.Role, r)));

        var identity = new ClaimsIdentity(claims, "Legacy");
        return new LegacySignInResult { Succeeded = true, Principal = new ClaimsPrincipal(identity) };
    }

    public async Task<LegacySignInResult> SignInCustomerAsync(string loginOrEmail, string password, CancellationToken cancellationToken = default)
    {
        var normalized = loginOrEmail.Trim().ToLowerInvariant();

        var customer = await _db.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c =>
                (c.WebshopLogin != null && c.WebshopLogin.ToLower() == normalized) ||
                c.CustomerEmail.ToLower() == normalized, cancellationToken);

        if (customer is null)
        {
            return new LegacySignInResult { Succeeded = false, Error = "Invalid login or password." };
        }

        if (!LegacyWebshopPasswordVerifier.Verify(password, customer.WebshopPasswordHash, customer.WebshopPasswordSalt))
        {
            return new LegacySignInResult { Succeeded = false, Error = "Invalid login or password." };
        }

        var displayName = string.IsNullOrWhiteSpace(customer.CustomerName)
            ? customer.WebshopLogin ?? customer.CustomerEmail
            : customer.CustomerName;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, $"customer:{customer.CustomerId}"),
            new Claim(ClaimTypes.Name, customer.WebshopLogin ?? customer.CustomerEmail),
            new Claim(ClaimTypes.Role, AppRoles.Customer),
            new Claim(LegacyAuthClaims.CustomerId, customer.CustomerId.ToString()),
            new Claim(LegacyAuthClaims.DisplayName, displayName),
            new Claim(LegacyAuthClaims.Login, customer.WebshopLogin ?? customer.CustomerEmail)
        };

        var identity = new ClaimsIdentity(claims, "Legacy");
        return new LegacySignInResult { Succeeded = true, Principal = new ClaimsPrincipal(identity) };
    }
}
