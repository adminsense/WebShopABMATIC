using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockPoReceiveContextDto
{
    public int StockOrderId { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
    public IReadOnlyList<StockPoReceiveLineDto> Lines { get; init; } = [];
    public IReadOnlyList<StockLookupItemDto> StockLocations { get; init; } = [];
}

public sealed class StockPoReceiveLineDto
{
    public int LineId { get; init; }
    public int? ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal QuantityOrdered { get; init; }
    public decimal QuantityDelivered { get; init; }
    public decimal QuantityRemaining { get; init; }
    public bool IsFullyDelivered { get; init; }
}

public sealed class StockPoReceivePreviewDto
{
    public int LineId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int StockLocationId { get; init; }
    public string StockLocationName { get; init; } = string.Empty;
    public decimal CurrentQuantity { get; init; }
    public decimal QuantityRemaining { get; init; }
}

public sealed class StockPoReceiveRequest : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int StockOrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int StockOrderLineId { get; set; }

    [Range(1, int.MaxValue)]
    public int StockLocationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string DeliveryDocumentNumber { get; set; } = string.Empty;

    [Required]
    public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.Date;

    [Range(0.0001, double.MaxValue)]
    public decimal Quantity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DeliveryDate.Date > DateTime.UtcNow.Date.AddDays(1))
        {
            yield return new ValidationResult(
                "Delivery date cannot be in the future.",
                [nameof(DeliveryDate)]);
        }
    }
}
