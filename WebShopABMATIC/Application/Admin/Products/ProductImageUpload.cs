namespace WebShopABMATIC.Application.Admin.Products;

public sealed class ProductImageUpload
{
    public required Stream Content { get; init; }
    public required string FileName { get; init; }
    public long SizeBytes { get; init; }
}

public static class ProductImageRules
{
    public const long MaxBytes = 1_048_576;

    public static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    public static bool IsAllowedExtension(string extension) =>
        AllowedExtensions.Contains(extension.StartsWith('.') ? extension : $".{extension}");
}
