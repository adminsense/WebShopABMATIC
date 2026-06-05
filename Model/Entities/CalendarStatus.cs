#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CalendarStatuses] (legacy: [Crm].[AgendaStatus]).</summary>
public class CalendarStatus
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Color { get; set; }
    public bool ShowInTodo { get; set; }
}

