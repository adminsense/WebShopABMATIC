#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CalendarLabels] (legacy: [Crm].[AgendaLabel]).</summary>
public class CalendarLabel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public bool IsType { get; set; }
    public bool IsCategory { get; set; }
    public bool? RestrictEditing { get; set; }
    public bool? IsForLeave { get; set; }
    public bool? IsInternalService { get; set; }
    public bool? IsExternalService { get; set; }
    public bool? IsProjectPlanning { get; set; }
}

