using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockLookupItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class StockAdjustmentLookupsDto
{
    public IReadOnlyList<StockLookupItemDto> StockLocations { get; init; } = [];
}

public sealed class StockAdjustmentPreviewDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int StockLocationId { get; init; }
    public string StockLocationName { get; init; } = string.Empty;
    public decimal CurrentQuantity { get; init; }
    public int ProductStockLocationId { get; init; }
}

public sealed class StockAdjustmentRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int StockLocationId { get; set; }

    [Required]
    public decimal QuantityChange { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Reason { get; set; } = string.Empty;
}
