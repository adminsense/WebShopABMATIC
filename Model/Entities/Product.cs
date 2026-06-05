#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[Product] (legacy: [Products].[Product]).</summary>
public class Product
{
    public int ProductId { get; set; }
    public string NameNl { get; set; }
    public string DescriptionNl { get; set; }
    public string OrderPartNumber { get; set; }
    public string StockNumber { get; set; }
    public int SupplierId { get; set; }
    public int ManufacturerId { get; set; }
    public int? ProductTypeId { get; set; }
    public bool IsInactive { get; set; }
    public decimal UnitsPerSale { get; set; }
    public decimal UnitsPerPurchase { get; set; }
    public int? PriceListSortOrder { get; set; }
    public bool ShowOnPriceList { get; set; }
    public string? ShortNotesNl { get; set; }
    public bool ProdToonOmschrijvingPrijslijst { get; set; }
    public decimal? RecupelAmount { get; set; }
    public decimal? BebatAmount { get; set; }
    public string? ProdRalKleur { get; set; }
    public string? ProdKleurPoedercode { get; set; }
    public decimal MinimumQuantity { get; set; }
    public decimal CustomWorkPercentage { get; set; }
    public int? RecupelProductId { get; set; }
    public int? BebatProductId { get; set; }
    public bool? IsQuickLooseSaleOption { get; set; }
    public string NameFr { get; set; }
    public string DescriptionFr { get; set; }
    public string ShortNotesFr { get; set; }
    public int? TaskTemplateId { get; set; }
    public int? PurchaseUnitId { get; set; }
    public int? SalesUnitId { get; set; }
    public int? AdsolutId { get; set; }
    public string NameEn { get; set; }
    public string DescriptionEn { get; set; }
    public string ShortNotesEn { get; set; }
    public bool? IsCompositeProduct { get; set; }
    public int? ProductStructureId { get; set; }
    public decimal? TemporaryDiscount { get; set; }
    public decimal? TemporaryNetPurchasePrice { get; set; }
    public int? ReportingGroupId { get; set; }
    public decimal SalesStockTriggerQuantity { get; set; }
    public decimal? ExtraPrice { get; set; }
    public decimal? ExtraAssemblyPrice { get; set; }
    public decimal? ExtraInstallationPrice { get; set; }
    public bool? IsProducedCompositeProduct { get; set; }
    public bool? IsVerified { get; set; }
    public bool HasNoPrice { get; set; }
    public bool ShowOnQuote { get; set; }
    public bool ShowOnOrderConfirmation { get; set; }
    public bool ShowOnInvoice { get; set; }
    public bool ShowOnPackingSlip { get; set; }
    public bool ShowOnDeliveryNote { get; set; }
    public bool ShowOnProductionOrder { get; set; }
    public bool ShowOnPaintShopOrder { get; set; }
    public bool ShowOnInstallationOrder { get; set; }
    public decimal Weight { get; set; }
    public string? InternalDocumentNotes { get; set; }
    public bool ExternalInstallerCost { get; set; }
    public bool ReportRecupel { get; set; }
    public bool ReportBebat { get; set; }
    public bool? HasTierPricing { get; set; }
    public bool? HideDetailPrice { get; set; }
    public bool? ShowOnWebshop { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool? IsNew { get; set; }
    public string? EanCode { get; set; }
    public string? PopupMessage { get; set; }
    public string WebshopDescriptionNl { get; set; }
    public string? GoodsCode { get; set; }
    public int? IntrastatCodeId { get; set; }
}

