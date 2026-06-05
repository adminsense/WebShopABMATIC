#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[PaymentMethods] (legacy: [Instellingen].[Betalingswijze]).</summary>
public class PaymentMethod
{
    public int Id { get; set; }
    public string NameNl { get; set; }
    public string NameFr { get; set; }
    public string NameEn { get; set; }
    public bool IsPrePay { get; set; }
    public bool IsPostPay { get; set; }
}

