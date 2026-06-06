#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[OrderTemplateDetail] (legacy: [Products].[OrderTemplateDetail]).</summary>
public class OrderTemplateDetail
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string GroupName { get; set; }
    public decimal Quantity { get; set; }
    public string QuantityFormula { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public int OrderTemplateId { get; set; }
    public string PriceFormula { get; set; }
    public int ProductEenheidId { get; set; }
}

