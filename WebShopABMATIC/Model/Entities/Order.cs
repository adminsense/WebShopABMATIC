#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[Orders] (legacy: [Projecten].[Bestelling]).</summary>
public class Order
{
    public int Id { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int ProjectId { get; set; }
    public decimal GeneralDiscount { get; set; }
    public int DeliveryTypeId { get; set; }
    public int PriceListTypeId { get; set; }
    public int? QuoteId { get; set; }
    public int? OrderNumber { get; set; }
    public decimal CommissionAmount { get; set; }
    public int? InstallerContactId { get; set; }
    public DateTime? RequestedDeliveryDate { get; set; }
    public int? InstallationDays { get; set; }
    public int? ProductionDays { get; set; }
    public int? MasterOrderId { get; set; }
    public int VatTypeId { get; set; }
    public int OrderProcessingTypeId { get; set; }
    public int CustomerTypeId { get; set; }
    public string? InternalNotes { get; set; }
    public string? RalColor { get; set; }
    public bool? AdvanceInvoiceEnabled { get; set; }
    public decimal? ExtraDiscount { get; set; }
    public string? CustomerNotes { get; set; }
    public string? InstallerNotes { get; set; }
    public string? InternalStaffNotes { get; set; }
    public bool AllowPartialDelivery { get; set; }
    public int? CommissionSalesUserId { get; set; }
    public bool? IsCommissionInvoiced { get; set; }
    public decimal? CommissionToInvoice { get; set; }
    public int? LeveradresId { get; set; }
    public int BetaaltermijnId { get; set; }
    public string? PopupMessage { get; set; }
    public int QuoteValidDays { get; set; }
    public bool IsUrgent { get; set; }
    public DateTime? PriceListDate { get; set; }
    public int BaseCompanyVatNumberId { get; set; }
    public bool? AdvancePaymentsByAmount { get; set; }
    public bool? HasCloudFolder { get; set; }
    public bool? IsClosingVerified { get; set; }
    public string? InvoiceNotes { get; set; }
    public string? QuoteNotesHeader { get; set; }
}

