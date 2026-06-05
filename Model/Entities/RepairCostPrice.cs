#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[RepairCostPrices] (legacy: [Instellingen].[HerstellingKostPrijs]).</summary>
public class RepairCostPrice
{
    public int Id { get; set; }
    public decimal Prijs { get; set; }
    public DateTime? ValidTo { get; set; }
    public DateTime FromAddress { get; set; }
}

