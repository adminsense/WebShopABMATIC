#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductType] (legacy: [Products].[ProductType]).</summary>
public class ProductType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameFr { get; set; }
    public bool IsStockItem { get; set; }
    public bool IsProduction { get; set; }
    public bool IsPurchaseItem { get; set; }
}

