#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Tasks].[TaskTemplateTasks] (legacy: [Taken].[TaakTemplateTaak]).</summary>
public class TaskTemplateTask
{
    public int Id { get; set; }
    public int SortOrder { get; set; }
    public long DueWithinTicks { get; set; }
    public int TaakTypeId { get; set; }
    public int? DefaultUserId { get; set; }
    public int? UseProjectOwnerAsDefault { get; set; }
    public int? DefaultUserGroupId { get; set; }
    public bool LockUntilPreviousComplete { get; set; }
    public int TaakTemplateId { get; set; }
    public string TaskName { get; set; }
}

