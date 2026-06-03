#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[StandardBillingTermLines] (legacy: [Instellingen].[StdFacturatieVoorwaardenDetail]).</summary>
public class StandardBillingTermLine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Percentage { get; set; }
    public int StdFacturatieVoorwaardenId { get; set; }
}

