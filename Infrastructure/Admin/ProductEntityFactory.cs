using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Data.Entities;

namespace WebShopABMATIC.Infrastructure.Admin;

internal static class ProductEntityFactory
{
    public static Product CreateNew(ProductEditDto dto) => new()
    {
        NameEn = dto.NameEn,
        NameNl = dto.NameEn,
        NameFr = dto.NameEn,
        DescriptionEn = dto.WebshopDescriptionNl,
        DescriptionNl = dto.WebshopDescriptionNl,
        DescriptionFr = dto.WebshopDescriptionNl,
        ShortNotesEn = string.Empty,
        ShortNotesNl = string.Empty,
        ShortNotesFr = string.Empty,
        OrderPartNumber = dto.OrderPartNumber,
        StockNumber = dto.OrderPartNumber,
        WebshopDescriptionNl = dto.WebshopDescriptionNl,
        SupplierId = dto.SupplierId,
        ManufacturerId = dto.ManufacturerId,
        ShowOnWebshop = dto.ShowOnWebshop,
        EanCode = dto.EanCode,
        IsInactive = false,
        UnitsPerSale = 1,
        UnitsPerPurchase = 1,
        ShowOnPriceList = true,
        ProdToonOmschrijvingPrijslijst = false,
        MinimumQuantity = 1,
        CustomWorkPercentage = 0,
        SalesStockTriggerQuantity = 0,
        HasNoPrice = false,
        ShowOnQuote = true,
        ShowOnOrderConfirmation = true,
        ShowOnInvoice = true,
        ShowOnPackingSlip = true,
        ShowOnDeliveryNote = true,
        ShowOnProductionOrder = false,
        ShowOnPaintShopOrder = false,
        ShowOnInstallationOrder = false,
        Weight = 0.5m,
        ExternalInstallerCost = false,
        ReportRecupel = false,
        ReportBebat = false
    };
}
