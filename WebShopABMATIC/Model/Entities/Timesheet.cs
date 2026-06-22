#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[Timesheet] (legacy: [Projecten].[Timesheet]).</summary>
public class Timesheet
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public DateTime StartDt { get; set; }
    public DateTime? StopDt { get; set; }
    public decimal? DurationMinutes { get; set; }
    public decimal? DurationHours { get; set; }
    public int JobCodeId { get; set; }
    public bool IsBillable { get; set; }
    public decimal? BillableMinutes { get; set; }
    public decimal? BillableHours { get; set; }
    public int PrestantUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public string Description { get; set; }
    public bool IsTimerRunning { get; set; }
    public DateTime? TimerStartedAt { get; set; }
    public int? TimerLastValue { get; set; }
}

