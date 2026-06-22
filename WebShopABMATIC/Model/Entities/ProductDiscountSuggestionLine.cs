#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[ProductDiscountSuggestionLines] (legacy: [Instellingen].[ProductKortingSuggestieDetail]).</summary>
public class ProductDiscountSuggestionLine
{
    public int Id { get; set; }
    public int CustomerTypeId { get; set; }
    public int ProductKortingSuggestieId { get; set; }
    public decimal Discount { get; set; }
}

