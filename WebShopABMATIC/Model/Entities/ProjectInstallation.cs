#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[ProjectInstallations] (legacy: [Projecten].[ProjectInstallatie]).</summary>
public class ProjectInstallation
{
    public int Id { get; set; }
    public int? InstallationOrderId { get; set; }
    public string? Location { get; set; }
    public string? Name { get; set; }
    public int ProjectId { get; set; }
    public string? SerialNumber { get; set; }
    public string? Specifications { get; set; }
}

