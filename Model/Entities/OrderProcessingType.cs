#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderProcessingTypes] (legacy: [Projecten].[DossierVerwerkingsType]).</summary>
public class OrderProcessingType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool CalculatePrice { get; set; }
    public int OrderStatusId { get; set; }
}

