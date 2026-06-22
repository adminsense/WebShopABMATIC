using Microsoft.AspNetCore.Identity;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    /// <summary>Linked domain customer (<c>Customers.Customers</c>) for store accounts.</summary>
    public int? CustomerId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string DisplayName => string.IsNullOrWhiteSpace(FirstName)
        ? (Email ?? UserName ?? "User")
        : $"{FirstName} {LastName}".Trim();
}
