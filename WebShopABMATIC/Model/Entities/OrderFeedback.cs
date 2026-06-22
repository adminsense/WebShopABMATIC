#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderFeedbacks] (legacy: [Projecten].[DossierFeedback]).</summary>
public class OrderFeedback
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Bucket { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public string? Text { get; set; }
}

