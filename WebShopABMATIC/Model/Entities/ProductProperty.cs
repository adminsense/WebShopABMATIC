#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductProperty] (legacy: [Products].[ProductProperty]).</summary>
public class ProductProperty
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
    public int SortOrder { get; set; }
}

