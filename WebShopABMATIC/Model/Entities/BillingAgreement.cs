#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[BillingAgreements] (legacy: [Projecten].[FacturatieAfspraak]).</summary>
public class BillingAgreement
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public decimal Percentage { get; set; }
    public string CustomerName { get; set; }
    public decimal VatPercentage { get; set; }
}

