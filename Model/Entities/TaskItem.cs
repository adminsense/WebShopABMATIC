#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[TaskItems] (legacy: [Crm].[Taken]).</summary>
public class TaskItem
{
    public DateTime ReminderDate { get; set; }
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string Description { get; set; }
    public int Type { get; set; }
    public int? AssignedUserId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ProjectId { get; set; }
    public int? OrderLineId { get; set; }
    public int? OrderId { get; set; }
    public int? BaseCompanyId { get; set; }
    public DateTime EndDate { get; set; }
    public int? PercentComplete { get; set; }
    public int? UserGroupId { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public bool? IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? CheckedByCreatorAt { get; set; }
    public bool? IsRead { get; set; }
    public bool? PopupShown { get; set; }
    public bool IsUrgent { get; set; }
    public string? RejectionReason { get; set; }
    public bool? IsRejectionRead { get; set; }
    public bool? IsRejected { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? RejectedAt { get; set; }
}

