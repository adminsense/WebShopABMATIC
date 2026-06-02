using Microsoft.AspNetCore.Identity;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string DisplayName => string.IsNullOrWhiteSpace(FirstName)
        ? (Email ?? UserName ?? "User")
        : $"{FirstName} {LastName}".Trim();
}
