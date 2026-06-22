#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductSubProduct] (legacy: [Products].[ProductSubProduct]).</summary>
public class ProductSubProduct
{
    public int Id { get; set; }
    public int MasterProductId { get; set; }
    public int SubProductId { get; set; }
    public decimal Quantity { get; set; }
    public bool IsOptional { get; set; }
    public decimal ExtraBasePrice { get; set; }
}

