#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[PriceListTexts] (legacy: [Products].[PrijslijstTeksten]).</summary>
public class PriceListText
{
    public int Id { get; set; }
    public int BaseCompanyId { get; set; }
    public string Text { get; set; }
    public string TextFr { get; set; }
    public string TextEn { get; set; }
}

