#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[StockMovements] (legacy: [Products].[StockBeweging]).</summary>
public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int? OrderLineId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Notes { get; set; }
    public bool? IsReservation { get; set; }
    public int ProductStockLocatieId { get; set; }
}

