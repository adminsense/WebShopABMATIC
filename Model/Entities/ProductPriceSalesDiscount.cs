#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPriceSalesDiscounts] (legacy: [Products].[ProductPrijzenVerkoopKorting]).</summary>
public class ProductPriceSalesDiscount
{
    public int Id { get; set; }
    public int KlantTypeId { get; set; }
    public int ProductPrijzenId { get; set; }
    public decimal Discount { get; set; }
}

