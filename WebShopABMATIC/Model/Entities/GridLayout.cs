#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[GridLayout] (legacy: [Instellingen].[GridLayout]).</summary>
public class GridLayout
{
    public int Id { get; set; }
    public string ObjectName { get; set; }
    public string LayoutXml { get; set; }
    public string? Notes { get; set; }
    public int? UsrId { get; set; }
    public bool? IsPivot { get; set; }
    public string? PivotName { get; set; }
}

