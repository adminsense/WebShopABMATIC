namespace WebShopABMATIC.Application.Store;

/// <summary>Category header content for the storefront (name + intro/outro price-list texts).</summary>
public sealed class StoreCategoryDetailDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? IntroText { get; init; }
    public string? OutroText { get; init; }

    public bool HasIntroText => !string.IsNullOrWhiteSpace(IntroText);
    public bool HasOutroText => !string.IsNullOrWhiteSpace(OutroText);
}
