#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Tasks].[TaskDependencies] (legacy: [Taken].[TaakDependency]).</summary>
public class TaskDependency
{
    public int? DependentTaskId { get; set; }
    public int Id { get; set; }
    public int? ParentTaskId { get; set; }
    public int Type { get; set; }
}

