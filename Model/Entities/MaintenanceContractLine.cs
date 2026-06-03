#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[MaintenanceContractLines] (legacy: [Projecten].[OnderhoudsContractDetail]).</summary>
public class MaintenanceContractLine
{
    public int Id { get; set; }
    public int OnderhoudsContractId { get; set; }
    public int ProjectInstallatieId { get; set; }
}

