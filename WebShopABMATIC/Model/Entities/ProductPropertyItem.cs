#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPropertyItems] (legacy: [Products].[ProductPropertieItem]).</summary>
public class ProductPropertyItem
{
    public int Id { get; set; }
    public int ProductPropertyId { get; set; }
    public string Value { get; set; }
    public int ProductId { get; set; }
}

