#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ServiceRates] (legacy: [Products].[PrestatieTarief]).</summary>
public class ServiceRate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }
}

