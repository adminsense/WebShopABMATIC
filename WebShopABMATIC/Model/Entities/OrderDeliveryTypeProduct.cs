#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderDeliveryTypeProducts] (legacy: [Projecten].[DossierLeveringsTypeProduct]).</summary>
public class OrderDeliveryTypeProduct
{
    public int Id { get; set; }
    public int LeveringTypeId { get; set; }
    public int ProductId { get; set; }
}

