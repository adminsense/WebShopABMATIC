#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[ProductDiscountSuggestions] (legacy: [Instellingen].[ProductKortingSuggestie]).</summary>
public class ProductDiscountSuggestion
{
    public int Id { get; set; }
    public decimal GrossCorrection { get; set; }
    public decimal Discount { get; set; }
    public decimal Pro1 { get; set; }
    public decimal Pro2 { get; set; }
    public decimal Pro3 { get; set; }
    public decimal Aan1 { get; set; }
    public decimal Aan2 { get; set; }
    public decimal Par1 { get; set; }
    public decimal DiscountCap { get; set; }
    public decimal Ond1 { get; set; }
}

