#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Tasks].[TaskTemplates] (legacy: [Taken].[TaakTemplate]).</summary>
public class TaskTemplate
{
    public int Id { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
}

