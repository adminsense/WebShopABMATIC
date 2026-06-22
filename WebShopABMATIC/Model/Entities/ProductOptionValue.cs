#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductOptionValue] (legacy: [Products].[ProductOptionValue]).</summary>
public class ProductOptionValue
{
    public int Id { get; set; }
    public int? OptieProduct { get; set; }
    public string? Value { get; set; }
    public int ProductOptionId { get; set; }
    public int SortOrder { get; set; }
    public string? ValueFr { get; set; }
    public string? ValueEn { get; set; }
}

