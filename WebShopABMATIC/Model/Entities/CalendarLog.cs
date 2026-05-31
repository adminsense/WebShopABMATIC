#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CalendarLogs] (legacy: [Crm].[AgendaLog]).</summary>
public class CalendarLog
{
    public int Id { get; set; }
    public string? Subject { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string ChangeAction { get; set; }
    public int? OrderId { get; set; }
    public int? ProjectId { get; set; }
    public int? CustomerId { get; set; }
    public int? ContactContactId { get; set; }
    public bool? IsLeave { get; set; }
    public DateTime ChangedAt { get; set; }
    public int UserId { get; set; }
    public int CalendarEntryId { get; set; }
    public int? ChangedByUserId { get; set; }
}

