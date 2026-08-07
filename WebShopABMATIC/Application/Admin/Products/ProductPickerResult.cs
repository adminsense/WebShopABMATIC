namespace WebShopABMATIC.Application.Admin.Products;

public sealed class ProductPickerResult
{
    public int ProductId { get; init; }
    public string NameEn { get; init; } = string.Empty;
    public string NameNl { get; init; } = string.Empty;
    public string? OrderPartNumber { get; init; }

    public string DisplayLabel
    {
        get
        {
            var name = !string.IsNullOrWhiteSpace(NameEn) ? NameEn
                : !string.IsNullOrWhiteSpace(NameNl) ? NameNl
                : "(unnamed)";
            return string.IsNullOrWhiteSpace(OrderPartNumber)
                ? $"#{ProductId} — {name}"
                : $"#{ProductId} — {name} ({OrderPartNumber})";
        }
    }

    public static string Format(int productId, string? nameEn, string? orderPartNumber = null)
    {
        if (productId <= 0)
        {
            return "No product selected";
        }

        var name = string.IsNullOrWhiteSpace(nameEn) ? "(unnamed)" : nameEn;
        return string.IsNullOrWhiteSpace(orderPartNumber)
            ? $"#{productId} — {name}"
            : $"#{productId} — {name} ({orderPartNumber})";
    }
}
