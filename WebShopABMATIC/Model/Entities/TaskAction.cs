#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[TaskActions] (legacy: [Crm].[TaakActies]).</summary>
public class TaskAction
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public DateTime Date { get; set; }
    public string Explanation { get; set; }
}

