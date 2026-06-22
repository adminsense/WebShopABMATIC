#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[MaintenanceContracts] (legacy: [Projecten].[OnderhoudsContract]).</summary>
public class MaintenanceContract
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal Amount { get; set; }
    public bool AutoRenew { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime? ReminderDate { get; set; }
    public int ProjectId { get; set; }
    public int CycleDays { get; set; }
    public int ReminderDaysInAdvance { get; set; }
    public int VatTypeId { get; set; }
    public int? SuccessorUserId { get; set; }
}

