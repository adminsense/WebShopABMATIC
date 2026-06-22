#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderInstallationLines] (legacy: [Projecten].[DossierInstallatieDetail]).</summary>
public class OrderInstallationLine
{
    public int Id { get; set; }
    public string ItemNumber { get; set; }
    public string PartNumber { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public decimal Quantity { get; set; }
    public decimal? QuantityInInstallation { get; set; }
    public string? Processing { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierArticleNumber { get; set; }
    public string? Treatment { get; set; }
    public DateTime? OrderedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? Notes { get; set; }
    public int OrderId { get; set; }
}

