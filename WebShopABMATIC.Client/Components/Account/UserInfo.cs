namespace WebShopABMATIC.Web.Components.Account;

internal sealed class UserInfo
{
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string[] Roles { get; set; } = [];
}
