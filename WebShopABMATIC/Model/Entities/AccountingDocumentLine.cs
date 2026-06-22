#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Accounting].[AccountingDocumentLines] (legacy: [Boekhouding].[DocumentDetail]).</summary>
public class AccountingDocumentLine
{
    public int Id { get; set; }
    public string GroupName { get; set; }
    public int? GateId { get; set; }
    public int? ProjectId { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductOmschrijving { get; set; }
    public decimal Quantity { get; set; }
    public decimal EenheidsPrijs { get; set; }
    public string Unit { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal Subtotaal { get; set; }
    public decimal BtwPercentage { get; set; }
    public decimal TotalAmount { get; set; }
    public int DocumentId { get; set; }
    public decimal InstallationPrice { get; set; }
    public decimal AssemblyPrice { get; set; }
    public bool IsOption { get; set; }
    public decimal BtwBedrag { get; set; }
    public decimal KortingBedrag { get; set; }
    public int SortOrder { get; set; }
    public string? PrijslijsType { get; set; }
    public string GroepNaam { get; set; }
    public int? IsOptieVanBestellingDetailId { get; set; }
    public int? ProductType { get; set; }
    public int? OrderLineId { get; set; }
    public int? GateComponentId { get; set; }
    public DateTime? LeveringAfhalingOkOp { get; set; }
    public decimal? NettoCommisieEenheidsPrijs { get; set; }
    public int OrderId { get; set; }
    public string? BestelNummer { get; set; }
    public decimal BasePrice { get; set; }
    public string KortingType { get; set; }
    public int? BebatProductId { get; set; }
    public int? RecupelProductId { get; set; }
    public string? BebatNaam { get; set; }
    public string? RecupelNaam { get; set; }
    public decimal BebatStukPrijs { get; set; }
    public decimal BebatAantal { get; set; }
    public decimal BebatTotaal { get; set; }
    public decimal RecupelStukPrijs { get; set; }
    public decimal RecupelAantal { get; set; }
    public decimal RecupelTotaal { get; set; }
    public decimal MontageStukPrijs { get; set; }
    public decimal AssemblageStukPrijs { get; set; }
    public bool? IsProducedCompositeProduct { get; set; }
    public bool? IsVoorschot { get; set; }
    public bool? IsTextOnly { get; set; }
    public DateTime? KlaarVoorVerzendingOp { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public bool NietTellenWegensVoorschot { get; set; }
    public bool IsGarantie { get; set; }
    public bool IsPopUpRow { get; set; }
    public string? Notes { get; set; }
    public decimal BasisPrijsTotaal { get; set; }
    public decimal StartupCost { get; set; }
    public decimal OpstartKostTotaal { get; set; }
    public int VatTypeId { get; set; }
    public int? SupplierId { get; set; }
    public int DocumentDetailMasterId { get; set; }
    public int? DetailVanMasterId { get; set; }
    public decimal? AankoopStukPrijs { get; set; }
    public decimal? Goederen { get; set; }
    public decimal? Diensten { get; set; }
    public string? GoodsCode { get; set; }
    public decimal? Weight { get; set; }
    public decimal? AanvullendeEenheden { get; set; }
    public int? LandVanOorsprong { get; set; }
}

