#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[BaseCompanyVatNumber] (legacy: [Instellingen].[BaseCompanyVatNumber]).</summary>
public class BaseCompanyVatNumber
{
    public int Id { get; set; }
    public int BaseCompanyId { get; set; }
    public string VatNumber { get; set; }
    public string EoriNumber { get; set; }
    public string? Bank1 { get; set; }
    public string? Bank2 { get; set; }
    public string? Bank1Name { get; set; }
    public string? Bank2Name { get; set; }
    public string? Bank1Bic { get; set; }
    public string? Bank2Bic { get; set; }
}

