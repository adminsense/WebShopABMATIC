#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPurchaseDiscounts] (legacy: [Products].[ProductAankoopKortingen]).</summary>
public class ProductPurchaseDiscount
{
    public int Id { get; set; }
    public decimal Percentage { get; set; }
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal Pro1 { get; set; }
    public decimal Pro2 { get; set; }
    public decimal Pro3 { get; set; }
    public decimal Aan1 { get; set; }
    public decimal Aan2 { get; set; }
    public decimal Par1 { get; set; }
    public DateTime? ValidTo { get; set; }
    public DateTime FromAddress { get; set; }
    public decimal Ond { get; set; }
}

