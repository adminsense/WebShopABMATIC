#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[TaskTypes] (legacy: [Crm].[TaakType]).</summary>
public class TaskType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal CompleteWithinDays { get; set; }
    public int? Color { get; set; }
    public bool ProductionWarning { get; set; }
    public bool DeliveryInstallationWarning { get; set; }
}

