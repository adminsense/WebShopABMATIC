#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[PriceListCategories] (legacy: [Products].[PrijslijstCategorie]).</summary>
public class PriceListCategory
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public string Name { get; set; }
    public bool? HasOptions { get; set; }
    public string? Color { get; set; }
    public string NameFr { get; set; }
}

