#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderAdvancePayments] (legacy: [Projecten].[DossierVoorschot]).</summary>
public class OrderAdvancePayment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Name { get; set; }
    public decimal Percent { get; set; }
    public bool IsFinalInvoice { get; set; }
    public DateTime? InvoicedAt { get; set; }
    public int SortOrder { get; set; }
    public decimal? Amount { get; set; }
    public string? AdvancePaymentVisibility { get; set; }
}

