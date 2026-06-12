namespace WebShopABMATIC.Application.Admin.StaffUsers;

public sealed class StaffUserDto
{
    public int Id { get; init; }
    public string Login { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? JobTitle { get; init; }
    public int? UserGroupId { get; init; }
    public string? Tel { get; init; }
}

public sealed class StaffUserEditDto
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? JobTitle { get; set; }
    public int? UserGroupId { get; set; }
    public string? Tel { get; set; }
}

public sealed class StaffUserListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
