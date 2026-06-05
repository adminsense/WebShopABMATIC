#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ReportingGroups] (legacy: [Products].[ReportingGroep1]).</summary>
public class ReportingGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
}

