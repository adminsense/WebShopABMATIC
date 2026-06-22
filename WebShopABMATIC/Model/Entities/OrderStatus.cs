#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderStatuses] (legacy: [Projecten].[BestellingStatus]).</summary>
public class OrderStatus
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? SortOrder { get; set; }
    public bool? IncludeInSalesReporting { get; set; }
    public string NameFr { get; set; }
    public bool? ReportInProgress { get; set; }
    public string ScreenMode { get; set; }
    public int? OrderStatusGroupId { get; set; }
    public bool ReserveStock { get; set; }
    public bool AffectsStock { get; set; }
}

