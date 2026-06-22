#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductOptions] (legacy: [Products].[ProductOptions]).</summary>
public class ProductOption
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ValueType { get; set; }
    public bool IsRequired { get; set; }
    public int ProductId { get; set; }
    public bool ProductionNotesFlag { get; set; }
    public bool QuoteNotesFlag { get; set; }
    public bool CalculatePrice { get; set; }
    public int SortOrder { get; set; }
    public bool? IsQuantityLine { get; set; }
    public string NameFr { get; set; }
    public string? DefaultValueFormula { get; set; }
    public string Tag { get; set; }
    public string? QuantityFormula { get; set; }
    public string NameEn { get; set; }
    public string? ExtraPriceFormula { get; set; }
    public string? UnitParameterFormula { get; set; }
}

