namespace WebShopABMATIC.Application.Store;

/// <summary>
/// Resolves the storefront product description from ERP fields (webshop NL first, then NL/EN/FR).
/// </summary>
public static class StoreProductDescription
{
    public static string Resolve(
        string? webshopDescriptionNl,
        string? descriptionNl,
        string? descriptionEn,
        string? descriptionFr)
    {
        if (!string.IsNullOrWhiteSpace(webshopDescriptionNl))
        {
            return webshopDescriptionNl.Trim();
        }

        if (!string.IsNullOrWhiteSpace(descriptionNl))
        {
            return descriptionNl.Trim();
        }

        if (!string.IsNullOrWhiteSpace(descriptionEn))
        {
            return descriptionEn.Trim();
        }

        if (!string.IsNullOrWhiteSpace(descriptionFr))
        {
            return descriptionFr.Trim();
        }

        return string.Empty;
    }
}
