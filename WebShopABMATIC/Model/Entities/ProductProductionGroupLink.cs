#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductProductionGroupLinks] (legacy: [Products].[ProductProductionsGroepen]).</summary>
public class ProductProductionGroupLink
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ProductionGroupId { get; set; }
}

