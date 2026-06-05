#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderTypes] (legacy: [Projecten].[BestellingType]).</summary>
public class OrderType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SerialNumberSuffix { get; set; }
    public int OrderProcessingTypeId { get; set; }
    public int SortOrder { get; set; }
    public string NameFr { get; set; }
}

