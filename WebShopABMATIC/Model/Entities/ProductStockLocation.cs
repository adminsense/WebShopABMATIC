#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductStockLocations] (legacy: [Products].[ProductStockLocatie]).</summary>
public class ProductStockLocation
{
    public int Id { get; set; }
    public int StockLocationId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public bool? IsInactive { get; set; }
    public decimal MaxQuantity { get; set; }
    public bool IsDefault { get; set; }
    public decimal MinQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public DateTime? LastCountedAt { get; set; }
    public decimal? CountedQuantity { get; set; }
}

