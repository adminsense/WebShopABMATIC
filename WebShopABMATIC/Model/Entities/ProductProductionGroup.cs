#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductProductionGroup] (legacy: [Products].[ProductProductionGroup]).</summary>
public class ProductProductionGroup
{
    public int ProductProductionGroupId { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public string Color { get; set; }
}

