namespace WebShopABMATIC.Application.Admin.UserAccounts;

public sealed class ApplicationUserAccountDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsManager { get; init; }
    public bool IsCustomer { get; init; }
    public bool IsActive { get; init; }
    public int? CustomerId { get; init; }
    public string AccountType { get; init; } = string.Empty;
}

public sealed class ApplicationUserAccountEditDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public bool IsCustomer { get; set; }
    public bool IsActive { get; set; } = true;
    public bool LockoutEnabled { get; set; } = true;
    public int? CustomerId { get; set; }

    /// <summary>Optional on create; when empty a temporary password is generated.</summary>
    public string? Password { get; set; }
}

public sealed class ApplicationUserAccountListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

public sealed class ApplicationUserAccountSaveResult
{
    public required bool Succeeded { get; init; }
    public string? UserId { get; init; }
    public string? TemporaryPassword { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = [];
}
