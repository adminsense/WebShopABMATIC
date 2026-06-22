#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerCustomProductLines] (legacy: [Crm].[KlantMaatProductDetail]).</summary>
public class CustomerCustomProductLine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? RawMaterialId { get; set; }
    public int? SupplierId { get; set; }
    public decimal Quantity { get; set; }
    public int ProductEenheidId { get; set; }
    public decimal? PurchasePrice { get; set; }
    public int CustomerCustomProductId { get; set; }
    public string? ArticleNumber { get; set; }
}

