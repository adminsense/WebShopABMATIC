#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Accounting].[VatTypes] (legacy: [Boekhouding].[BtwType]).</summary>
public class VatType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string InvoiceText { get; set; }
    public decimal Percentage { get; set; }
    public string InvoiceTextEn { get; set; }
    public string InvoiceTextFr { get; set; }
    public string ExplanationNl { get; set; }
    public string ExplanationFr { get; set; }
    public string ExplanationEn { get; set; }
    public bool? IsDefault { get; set; }
    public string? TaxExemptionReason { get; set; }
    public string? TaxExemptionReasonCode { get; set; }
    public string? PeppolCode { get; set; }
}

