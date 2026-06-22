#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPopupValueTypes] (legacy: [Products].[ProductPopupWaardeType]).</summary>
public class ProductPopupValueType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameFr { get; set; }
    public string Description { get; set; }
    public string DescriptionFr { get; set; }
    public string Tag { get; set; }
}

