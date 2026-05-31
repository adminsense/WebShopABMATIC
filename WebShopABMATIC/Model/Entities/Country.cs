#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[Country] (legacy: [Crm].[Country]).</summary>
public class Country
{
    public int Id { get; set; }
    public bool? IsEu { get; set; }
    public string IsoCode { get; set; }
    public string? Name { get; set; }
}

