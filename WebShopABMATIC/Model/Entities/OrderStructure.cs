#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderStructures] (legacy: [Projecten].[DossierStructuur]).</summary>
public class OrderStructure
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public int? MasterRowId { get; set; }
    public int? OrderTypeId { get; set; }
    public int? OrderId { get; set; }
    public int SortOrder { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int StatusId { get; set; }
    public string CustomerReference { get; set; }
    public string? OrderNumber { get; set; }
    public string? Path { get; set; }
    public DateTime? OrderConfirmationDate { get; set; }
    public DateTime? PlannedDate { get; set; }
    public DateTime? ReadyForDeliveryDate { get; set; }
    public DateTime? ReadyForBillingDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? QuoteDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? TotalInvoiced { get; set; }
    public string? TeamsId { get; set; }
    public string? OrderConfirmedBy { get; set; }
    public string? PlannedBy { get; set; }
    public string? ReadyForDeliveryBy { get; set; }
    public string? CompletedBy { get; set; }
    public string? QuoteBy { get; set; }
    public string? ReadyForBillingBy { get; set; }
    public bool HasPaperFile { get; set; }
    public DateTime? QueuedAt { get; set; }
}

