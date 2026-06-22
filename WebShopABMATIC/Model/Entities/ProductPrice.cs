#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductPrices] (legacy: [Products].[ProductPrijzen]).</summary>
public class ProductPrice
{
    public int Id { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal AssemblyPrice { get; set; }
    public decimal InstallationPrice { get; set; }
    public int ProductId { get; set; }
    public decimal GrossSalesPrice { get; set; }
    public decimal GrossPurchasePrice { get; set; }
    public decimal NetPurchasePrice { get; set; }
    public int? ProductAankoopKortingenId { get; set; }
    public bool? OverrideBruto { get; set; }
    public decimal BasePrice { get; set; }
    public decimal CorrectedGrossPrice { get; set; }
    public string? PriceCalculationFormula { get; set; }
    public bool? UsePriceCalculationFormula { get; set; }
    public string? BasePriceCalculationFormula { get; set; }
    public decimal StartupCost { get; set; }
    public decimal Pro1 { get; set; }
    public decimal Pro2 { get; set; }
    public decimal Pro3 { get; set; }
    public decimal Aan1 { get; set; }
    public decimal Aan2 { get; set; }
    public decimal Par1 { get; set; }
    public decimal Ond { get; set; }
    public decimal? ExtraPurchaseCost { get; set; }
    public string? ExtraPurchaseCostNotes { get; set; }
    public decimal PurchaseDiscountPercentage { get; set; }
    public decimal GrossCorrectionPercentage { get; set; }
    public int CalculationType { get; set; }
    public bool SupplierUsesDifferentGrossSalesPrice { get; set; }
}

