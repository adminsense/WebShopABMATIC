using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.SystemUsers;

public sealed class SystemUserDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsManager { get; init; }
    public bool IsActive { get; init; }
    public bool LockoutEnabled { get; init; }
}

public sealed class SystemUserEditDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public bool IsActive { get; set; } = true;
    public bool LockoutEnabled { get; set; } = true;

    /// <summary>Optional on create; when empty a temporary password is generated.</summary>
    public string? Password { get; set; }
}

public sealed class SystemUserListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}

public sealed class SystemUserSaveResult
{
    public required bool Succeeded { get; init; }
    public string? UserId { get; init; }
    public string? TemporaryPassword { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}
