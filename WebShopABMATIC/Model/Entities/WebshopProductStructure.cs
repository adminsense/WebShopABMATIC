#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[WebshopProductStructures] (legacy: [Products].[ProductStructuurWebshop]).</summary>
public class WebshopProductStructure
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
    public int? ParentTaskId { get; set; }
}

