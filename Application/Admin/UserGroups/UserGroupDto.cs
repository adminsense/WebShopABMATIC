namespace WebShopABMATIC.Application.Admin.UserGroups;

public sealed class UserGroupDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsInstallationTeam { get; init; }
    public bool IsServiceTeam { get; init; }
    public bool IsTransportTeam { get; init; }
}

public sealed class UserGroupEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsInstallationTeam { get; set; }
    public bool IsServiceTeam { get; set; }
    public bool IsTransportTeam { get; set; }
}

public sealed class UserGroupListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
