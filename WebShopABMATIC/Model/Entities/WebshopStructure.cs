#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[WebshopStructures] (legacy: [Products].[WebshopStructuur]).</summary>
public class WebshopStructure
{
    public int Id { get; set; }
    public string NameNl { get; set; }
    public int? ParentTaskId { get; set; }
    public int SortOrder { get; set; }
}

