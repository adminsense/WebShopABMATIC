#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[OrderTemplate] (legacy: [Products].[OrderTemplate]).</summary>
public class OrderTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int? CustomerId { get; set; }
}

