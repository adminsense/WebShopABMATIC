#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[BaseCompany] (legacy: [Instellingen].[BaseCompany]).</summary>
public class BaseCompany
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Street { get; set; }
    public string StreetNr { get; set; }
    public string StreetBox { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public int? CustomerId { get; set; }
    public byte[]? Logo { get; set; }
    public string VatNumber { get; set; }
    public string Tel { get; set; }
    public string FaxTemplate { get; set; }
    public string IBAN { get; set; }
    public string BIC { get; set; }
    public string Slogan { get; set; }
    public string AccountingDocumentFooter { get; set; }
    public string Tag { get; set; }
    public string? MsGraphApiApplicationId { get; set; }
    public string? MsGraphApiSecretId { get; set; }
    public string? MsGraphApiDomain { get; set; }
    public string? MsGraphApiTenantId { get; set; }
    public string? FileShareComputername { get; set; }
    public string? FileShareUser { get; set; }
    public string? FileSharePassword { get; set; }
    public string? FileShareShare { get; set; }
    public string? FileShareAccountName { get; set; }
    public string? Eori { get; set; }
    public string? Website { get; set; }
    public int? AccentColor { get; set; }
    public string Bank { get; set; }
    public string? EmailTemplate { get; set; }
    public bool? AllowAddNewBlobFiles { get; set; }
}

