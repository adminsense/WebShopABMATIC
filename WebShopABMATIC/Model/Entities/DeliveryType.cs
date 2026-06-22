#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[DeliveryTypes] (legacy: [Projecten].[LeveringType]).</summary>
public class DeliveryType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IncludeInstallationCost { get; set; }
    public string NameFr { get; set; }
    public bool? IsDefault { get; set; }
}

