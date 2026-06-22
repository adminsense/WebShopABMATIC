#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[StockLocations] (legacy: [Products].[StockLocatie]).</summary>
public class StockLocation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool? IsWarehouse { get; set; }
}

