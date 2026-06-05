namespace WebShopABMATIC.Web.Services;

public static class StoreCategories
{
    public static IReadOnlyList<(string Key, string Label)> All { get; } =
    [
        ("all", "All"),
        ("storage", "Storage"),
        ("ssd", "SSD"),
        ("hdd", "HDD")
    ];
}
