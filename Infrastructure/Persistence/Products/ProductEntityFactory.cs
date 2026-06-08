using WebShopABMATIC.Domain.Catalog.Products;
using PersistenceProduct = WebShopABMATIC.Data.Entities.Product;

namespace WebShopABMATIC.Infrastructure.Persistence.Products;

internal static class ProductEntityFactory
{
    public static PersistenceProduct CreateNew(Product product) => new()
    {
        NameEn = product.NameEn,
        NameNl = product.NameEn,
        NameFr = product.NameEn,
        DescriptionEn = product.WebshopDescriptionNl,
        DescriptionNl = product.WebshopDescriptionNl,
        DescriptionFr = product.WebshopDescriptionNl,
        ShortNotesEn = string.Empty,
        ShortNotesNl = string.Empty,
        ShortNotesFr = string.Empty,
        OrderPartNumber = product.OrderPartNumber,
        StockNumber = product.OrderPartNumber,
        WebshopDescriptionNl = product.WebshopDescriptionNl,
        SupplierId = product.SupplierId,
        ManufacturerId = product.ManufacturerId,
        ShowOnWebshop = product.ShowOnWebshop,
        EanCode = product.EanCode,
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
