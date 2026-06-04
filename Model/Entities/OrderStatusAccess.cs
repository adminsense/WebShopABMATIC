#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderStatusAccesses] (legacy: [Projecten].[BestellingStatusToegangen]).</summary>
public class OrderStatusAccess
{
    public int Id { get; set; }
    public int OrderStatusId { get; set; }
    public int UserId { get; set; }
}

