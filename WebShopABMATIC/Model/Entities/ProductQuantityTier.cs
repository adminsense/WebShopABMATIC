#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductQuantityTiers] (legacy: [Products].[ProductStaffel]).</summary>
public class ProductQuantityTier
{
    public int Id { get; set; }
    public decimal MinimumQuantity { get; set; }
    public decimal Discount { get; set; }
    public int ProductId { get; set; }
}

