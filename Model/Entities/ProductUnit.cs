#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductUnits] (legacy: [Products].[ProductEenheid]).</summary>
public class ProductUnit
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
    public bool UnitParameter { get; set; }
}

