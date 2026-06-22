#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[StandardBillingTerms] (legacy: [Instellingen].[StdFacturatieVoorwaarden]).</summary>
public class StandardBillingTerm
{
    public int Id { get; set; }
    public string Name { get; set; }
}

