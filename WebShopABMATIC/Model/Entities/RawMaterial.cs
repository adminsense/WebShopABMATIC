#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[RawMaterials] (legacy: [Products].[Grondstof]).</summary>
public class RawMaterial
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

