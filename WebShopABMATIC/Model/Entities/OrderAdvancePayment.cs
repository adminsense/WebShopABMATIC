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

    /// <summary>Not an ERP column — derived from <see cref="Name"/> / <see cref="AdvancePaymentVisibility"/> / <see cref="InvoicedAt"/>.</summary>
    public string? MolliePaymentId { get; set; }
    /// <summary>Not an ERP column — maps to <see cref="AdvancePaymentVisibility"/>.</summary>
    public string? MolliePaymentStatus { get; set; }
    /// <summary>Not an ERP column — maps to <see cref="InvoicedAt"/>.</summary>
    public DateTime? MolliePaidAt { get; set; }
    /// <summary>Not an ERP column — never persisted (redirect URL is ephemeral).</summary>
    public string? MollieCheckoutUrl { get; set; }
}

