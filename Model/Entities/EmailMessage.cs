#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Emails].[EmailMessages] (legacy: [Emails].[Email]).</summary>
public class EmailMessage
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? ProjectId { get; set; }
    public string? FromAddress { get; set; }
    public string? ToAddress { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime ReceivedAt { get; set; }
    public int? ContactId { get; set; }
    public int? RelatedSupplierId { get; set; }
    public int? OrderId { get; set; }
    public string PreviewText { get; set; }
    public int? TaskItemId { get; set; }
    public int? UserId { get; set; }
    public bool? IsPrivate { get; set; }
    public int? SupplierId { get; set; }
    public int? ManufacturerId { get; set; }
    public int? OrderLineId { get; set; }
    public bool? RequiresAction { get; set; }
    public int? EmailQueueId { get; set; }
    public int? StockOrderId { get; set; }
}

