#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[MiscellaneousProducts] (legacy: [Products].[LosseProducten]).</summary>
public class MiscellaneousProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? ArticleNumber { get; set; }
    public string? StockLocationCode { get; set; }
    public string? Notes { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? SupplierName { get; set; }
    public decimal? AantalInStock { get; set; }
    public DateTime? LastCountedAt { get; set; }
    public string? GroupName { get; set; }
}

