#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[StockOrder] (legacy: [Products].[StockOrder]).</summary>
public class StockOrder
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? OrderConfirmationDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
    public int UserId { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? InternalNotes { get; set; }
    public decimal? TotalAmount { get; set; }
}

