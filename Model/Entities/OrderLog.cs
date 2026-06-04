#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderLogs] (legacy: [Projecten].[DossierLog]).</summary>
public class OrderLog
{
    public long Id { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
}

