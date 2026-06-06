#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Accounting].[AccountingDocuments] (legacy: [Boekhouding].[Documenten]).</summary>
public class AccountingDocument
{
    public DateTime DocumentCreatedAt { get; set; }
    public decimal DocumentVatAmount { get; set; }
    public decimal DocumentNetAmount { get; set; }
    public decimal DocumentTotalAmount { get; set; }
    public DateTime DocumentDate { get; set; }
    public bool? IsFinal { get; set; }
    public int AccountingDocumentId { get; set; }
    public string DocumentCustomerBox { get; set; }
    public int CustomerId { get; set; }
    public string DocumentCustomerName { get; set; }
    public string DocumentCustomerNumber { get; set; }
    public string DocumentCustomerPostalCode { get; set; }
    public string DocumentCustomerStreet { get; set; }
    public string DocumentCustomerCity { get; set; }
    public string? DocumentNumber { get; set; }
    public int DocumentTypeId { get; set; }
    public int CreatedBy { get; set; }
    public int? OrderId { get; set; }
    public string DocumentCustomerCompanyName { get; set; }
    public string DocumentCustomerVatNumber { get; set; }
    public string DocumentCustomerCountry { get; set; }
    public string DocumentCustomerHouseNumber { get; set; }
    public DateTime? Vervaldatum { get; set; }
    public string? GecrediteerdeFactuur { get; set; }
    public DateTime? DocGecrediteerdeFactuurDatum { get; set; }
    public string? ProjectContactNaam { get; set; }
    public string? ProjectContactPhone { get; set; }
    public string? ProjectContactMobile { get; set; }
    public string? ProjectContactEmail { get; set; }
    public string? LeverAdresContactNaam { get; set; }
    public string? LeverAdresContactPhone { get; set; }
    public string? LeverAdresContactMobile { get; set; }
    public string? LeverAdresContactEmail { get; set; }
    public string? EindklantContactNaam { get; set; }
    public string? EindklantContactPhone { get; set; }
    public string? EindklantContactMobile { get; set; }
    public string? EindklantContactEmail { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? BaseCompanyId { get; set; }
    public int? DocumentCustomerLanguageId { get; set; }
    public int ProjectId { get; set; }
    public string? LeverAdresStraat { get; set; }
    public string? LeverAdresNr { get; set; }
    public string? LeverAdresBus { get; set; }
    public string? LeverAdresStad { get; set; }
    public string? LeverAdresPostcode { get; set; }
    public string? Notes { get; set; }
    public string? LeverAdresLand { get; set; }
    public bool? IsVoorschotFactuur { get; set; }
    public decimal? ReedsGefactureerdVoorschot { get; set; }
    public string? VoorschotNaam { get; set; }
    public bool? HeeftCommisie { get; set; }
    public decimal? VoorschotPercentage { get; set; }
    public string? VerzondenVia { get; set; }
    public string? DossierBeheerder { get; set; }
    public string? ProjectManagerUserId { get; set; }
    public string? AccountManagerUserId { get; set; }
    public DateTime? BetaaldOp { get; set; }
    public int? BetalingswijzeId { get; set; }
    public string? Reason { get; set; }
    public string? ToelichtingVoorschotten { get; set; }
    public int BaseCompanyVatNumberId { get; set; }
    public string? DocKlantGebouwNaam { get; set; }
    public string? DocumentOpmerking { get; set; }
    public int? CountryId { get; set; }
    public DateTime? PeppolVerzondenOp { get; set; }
    public string? EasypostId { get; set; }
    public string? PeppolStatus { get; set; }
}

