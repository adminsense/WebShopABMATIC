using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockTransferLookupsDto
{
    public IReadOnlyList<StockLookupItemDto> StockLocations { get; init; } = [];
}

public sealed class StockTransferPreviewDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int FromStockLocationId { get; init; }
    public string FromStockLocationName { get; init; } = string.Empty;
    public decimal FromCurrentQuantity { get; init; }
    public int ToStockLocationId { get; init; }
    public string ToStockLocationName { get; init; } = string.Empty;
    public decimal ToCurrentQuantity { get; init; }
}

public sealed class StockTransferRequest : IValidatableObject
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int FromStockLocationId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ToStockLocationId { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Reason { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FromStockLocationId == ToStockLocationId)
        {
            yield return new ValidationResult(
                "From and to stock locations must be different.",
                [nameof(ToStockLocationId), nameof(FromStockLocationId)]);
        }
    }
}
