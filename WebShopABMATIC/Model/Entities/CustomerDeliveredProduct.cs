#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[CustomerDeliveredProducts] (legacy: [Projecten].[BinnengebrachtProduct]).</summary>
public class CustomerDeliveredProduct
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Name { get; set; }
    public string Explanation { get; set; }
    public DateTime ReceivedAt { get; set; }
    public int ReceivedByUserId { get; set; }
    public string BroughtBy { get; set; }
    public DateTime? PickedUpAt { get; set; }
    public string? PickedUpBy { get; set; }
    public DateTime? SentToSupplierAt { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime? ReturnedFromSupplierAt { get; set; }
    public DateTime? IrreparableAt { get; set; }
}

