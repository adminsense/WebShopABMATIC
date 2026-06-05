#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPopupTemplateLines] (legacy: [Products].[ProductPopupTemplateDetail]).</summary>
public class ProductPopupTemplateLine
{
    public int Id { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
    public string TransferToQuantityFormula { get; set; }
    public bool TransferToQuantity { get; set; }
    public bool IncludePrice { get; set; }
    public int WriteToLineColumn { get; set; }
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }
}

