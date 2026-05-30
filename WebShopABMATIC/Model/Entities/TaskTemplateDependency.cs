#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Tasks].[TaskTemplateDependencies] (legacy: [Taken].[TaakTemplateDependencie]).</summary>
public class TaskTemplateDependency
{
    public int Id { get; set; }
    public int? ParentTaskId { get; set; }
    public int? DependsOnTaskTemplateTaskId { get; set; }
    public int Type { get; set; }
}

