#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[PaymentTerms] (legacy: [Crm].[Betaaltermijn]).</summary>
public class PaymentTerm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AantalDagen { get; set; }
    public bool EndOfMonth { get; set; }
    public bool? IsDefault { get; set; }
}

