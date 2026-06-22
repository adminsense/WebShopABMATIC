namespace WebShopABMATIC.Infrastructure.Media;

/// <summary>
/// Maps legacy <see cref="WebShopABMATIC.Data.Entities.AzureFile.BlobRef"/> values to blob object names.
/// Production examples: <c>19-10-2022-10-58-4567322bc1-…</c> with <c>Extension</c> <c>.jpg</c>.
/// </summary>
public static class BlobReferenceResolver
{
    public static bool IsAbsoluteUrl(string? value) =>
        !string.IsNullOrWhiteSpace(value)
        && (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

    public static bool IsLocalMediaPath(string? value) =>
        !string.IsNullOrWhiteSpace(value)
        && (value.StartsWith("/media/", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("media/", StringComparison.OrdinalIgnoreCase));

    public static IReadOnlyList<string> BuildBlobNameCandidates(string blobRef, string? extension)
    {
        var candidates = new List<string> { blobRef.Trim() };

        var normalizedExtension = NormalizeExtension(extension);
        if (normalizedExtension is null)
        {
            return candidates;
        }

        if (!blobRef.EndsWith(normalizedExtension, StringComparison.OrdinalIgnoreCase))
        {
            candidates.Add(blobRef + normalizedExtension);
        }

        if (!blobRef.Contains('.') && !string.IsNullOrEmpty(normalizedExtension.TrimStart('.')))
        {
            candidates.Add($"{blobRef}.{normalizedExtension.TrimStart('.')}");
        }

        return candidates.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>
    /// ABMATIC stores blobs under the raw <c>BlobRef</c> key; <c>Extension</c> is metadata only.
    /// </summary>
    public static string ResolvePreferredBlobName(string blobRef, string? extension) =>
        blobRef.Trim();

    public static string BuildLegacyBlobKey(string extension)
    {
        var normalized = NormalizeExtension(extension) ?? ".jpg";
        var now = DateTime.Now;
        return $"{now.Day}-{now.Month}-{now.Year}-{now.Hour}-{now.Minute}-{now:fffffff}-{Guid.NewGuid():N}{normalized}";
    }

    private static string? NormalizeExtension(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return null;
        }

        var trimmed = extension.Trim();
        return trimmed.StartsWith('.') ? trimmed : $".{trimmed}";
    }
}
