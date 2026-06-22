namespace WebShopABMATIC.Infrastructure.Media;

public sealed class AzureStorageOptions
{
    public const string SectionName = "AzureStorage";

    public string? ConnectionString { get; set; }

    /// <summary>Blob container for product images (account <c>abmatic</c>: <c>files</c>).</summary>
    public string ContainerName { get; set; } = "files";

    /// <summary>When false, use anonymous blob URL (container must allow public read).</summary>
    public bool UseSasUrls { get; set; } = true;

    public int SasValidityHours { get; set; } = 12;
}
