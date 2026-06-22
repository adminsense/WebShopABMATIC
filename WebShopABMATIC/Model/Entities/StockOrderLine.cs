#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[StockOrderLines] (legacy: [Products].[StockOrderDetail]).</summary>
public class StockOrderLine
{
    public int Id { get; set; }
    public int StockOrderId { get; set; }
    public int? ProductId { get; set; }
    public decimal QuantityOrdered { get; set; }
    public bool LijnOK { get; set; }
    public string ProductName { get; set; }
    public string OrderNumber { get; set; }
    public string PackSize { get; set; }
    public decimal PurchaseUnitPrice { get; set; }
    public decimal PurchaseTotalPrice { get; set; }
    public string Unit { get; set; }
    public string? InternalReference { get; set; }
    public string? DeliveryNotes { get; set; }
    public string? OrderNotes { get; set; }
    public bool? Besteld { get; set; }
    public DateTime? OrderedAt { get; set; }
    public bool? Geleverd { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public int? ProductTypeId { get; set; }
    public decimal QuantityDelivered { get; set; }
    public decimal? QuantityProcessedToStock { get; set; }
}

