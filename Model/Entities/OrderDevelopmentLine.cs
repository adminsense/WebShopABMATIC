#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderDevelopmentLines] (legacy: [Projecten].[DossierDevelopmentDetail]).</summary>
public class OrderDevelopmentLine
{
    public Guid Id { get; set; }
    public string ArticleNumber { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? ProductId { get; set; }
    public Guid? MasterRowId { get; set; }
    public decimal? Quantity { get; set; }
    public int? ProductEenheidId { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal QuantityTakenFromStock { get; set; }
    public bool MustOrder { get; set; }
    public DateTime? OrderedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public int? SupplierId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal TotalPurchasePrice { get; set; }
    public bool DrawingCreated { get; set; }
    public string? MaterialType { get; set; }
    public string? Processing { get; set; }
    public int OrderId { get; set; }
    public int SortOrder { get; set; }
    public decimal? Weight { get; set; }
    public string? Finish { get; set; }
    public int? StockOrderDetailId { get; set; }
    public bool? RequiresPainting { get; set; }
    public int? PaintWorkSupplierId { get; set; }
}

