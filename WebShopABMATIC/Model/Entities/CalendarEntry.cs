#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CalendarEntries] (legacy: [Crm].[Agenda]).</summary>
public class CalendarEntry
{
    public int Id { get; set; }
    public int? Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsAllDay { get; set; }
    public string? Subject { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public int? StatusId { get; set; }
    public int? LabelId { get; set; }
    public string? ReminderInfo { get; set; }
    public string? RecurrenceInfo { get; set; }
    public string? OutlookId { get; set; }
    public int? UserId { get; set; }
    public int? CustomerId { get; set; }
    public int? ProjectId { get; set; }
    public bool IsSyncedToExchange { get; set; }
    public int? OrderId { get; set; }
    public bool? IsOnHold { get; set; }
    public bool? IsCancelled { get; set; }
    public string? OnHoldOrCancelledReason { get; set; }
    public string? SubjectUserText { get; set; }
    public int? ContactContactId { get; set; }
    public string? SystemDescription { get; set; }
    public bool? IsLeave { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

