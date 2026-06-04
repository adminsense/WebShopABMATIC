#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderStatusGroups] (legacy: [Projecten].[DossierStatusGroep]).</summary>
public class OrderStatusGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
}

