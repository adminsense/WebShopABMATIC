#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerCustomProductTiers] (legacy: [Crm].[KlantMaatproductStaffel]).</summary>
public class CustomerCustomProductTier
{
    public int Id { get; set; }
    public int CustomerCustomProductId { get; set; }
    public decimal MinQuantity { get; set; }
    public decimal MaxQuantity { get; set; }
    public decimal PieceUnitPrice { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}

