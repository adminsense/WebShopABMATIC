#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Logging].[ProjectActivities] (legacy: [Logging].[ProjectActiviteit]).</summary>
public class ProjectActivity
{
    public int ProjectId { get; set; }
    public int ActionCode { get; set; }
    public int Id { get; set; }
    public DateTime LoggedAt { get; set; }
}

