namespace WebShopABMATIC.Application.Store;

/// <summary>Configurable option for a webshop product (read-only projection of Products.ProductOptions).</summary>
public sealed class StoreProductOptionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ValueType { get; init; } = string.Empty;
    public bool IsRequired { get; init; }
    public int SortOrder { get; init; }
    public IReadOnlyList<StoreProductOptionValueDto> Values { get; init; } = [];

    /// <summary>True when the option offers a fixed list of values (rendered as a dropdown).</summary>
    public bool HasValues => Values.Count > 0;
}

/// <summary>Selectable value for a <see cref="StoreProductOptionDto"/> (read-only projection of Products.ProductOptionValue).</summary>
public sealed class StoreProductOptionValueDto
{
    public int Id { get; init; }
    public string Value { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}
