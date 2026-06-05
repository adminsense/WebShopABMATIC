#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[ProjectLog] (legacy: [Projecten].[ProjectLog]).</summary>
public class ProjectLog
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ActionCode { get; set; }
    public DateTime Date { get; set; }
}

