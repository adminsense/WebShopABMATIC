#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[Project] (legacy: [Projecten].[Project]).</summary>
public class Project
{
    public int ProjectId { get; set; }
    public int ProjectNumber { get; set; }
    public string ProjectName { get; set; }
    public int ProjectManagerUserId { get; set; }
    public int? CustomerId { get; set; }
    public int? JobSiteId { get; set; }
    public int ProjectTypeId { get; set; }
    public DateTime? ProjectCreatedAt { get; set; }
    public string? ProjectInternalNotes { get; set; }
    public int? BaseCompanyId { get; set; }
    public string? ProductionLabelReference { get; set; }
    public bool? IsTemplate { get; set; }
    public string? ProjectNotes { get; set; }
    public bool IsStandardProject { get; set; }
    public string? TeamsId { get; set; }
    public string? PopupMessage { get; set; }
}

