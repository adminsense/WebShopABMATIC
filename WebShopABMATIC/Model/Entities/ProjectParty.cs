#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[ProjectParties] (legacy: [Projecten].[ProjectPartij]).</summary>
public class ProjectParty
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int? ProjectPartijGroepId { get; set; }
    public string? Comment { get; set; }
    public decimal BillingPercentage { get; set; }
    public int ProjectId { get; set; }
}

