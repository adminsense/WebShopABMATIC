#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPopupReturnColumns] (legacy: [Products].[ProductPopupRetourKolom]).</summary>
public class ProductPopupReturnColumn
{
    public int Id { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
    public string LooseSaleColumn { get; set; }
    public string GateComponentColumn { get; set; }
}

