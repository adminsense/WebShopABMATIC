#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductStructures] (legacy: [Products].[ProductStructuur]).</summary>
public class ProductStructure
{
    public int Id { get; set; }
    public int Level { get; set; }
    public int? ParentTaskId { get; set; }
    public string NameNl { get; set; }
    public string NameEn { get; set; }
    public string NameFr { get; set; }
    public int? IntroPriceListTextId { get; set; }
    public int? OutroPriceListTextId { get; set; }
    public int SortOrder { get; set; }
    public int? Color { get; set; }
    public bool? ShowOnPriceList { get; set; }
    public byte[]? Icon { get; set; }
    public bool PageBreakAfter { get; set; }
}

