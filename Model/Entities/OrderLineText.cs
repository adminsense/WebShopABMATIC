#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[OrderLineTexts] (legacy: [Projecten].[DossierDetailText]).</summary>
public class OrderLineText
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool OrderBevestiging { get; set; }
    public bool Leveringsbon { get; set; }
    public bool Factuur { get; set; }
    public bool AkOrder { get; set; }
    public bool Offerte { get; set; }
    public int? DossierDetailsId { get; set; }
    public bool MontageBon { get; set; }
    public bool Lakbon { get; set; }
    public bool ProductieBon { get; set; }
}

