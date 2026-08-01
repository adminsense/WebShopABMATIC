using WebShopABMATIC.Domain.Catalog.Products;
using PersistenceProduct = WebShopABMATIC.Data.Entities.Product;

namespace WebShopABMATIC.Infrastructure.Persistence.Mappers;

internal static class ProductPersistenceMapper
{
    public static Product ToDomain(PersistenceProduct entity) =>
        Product.Rehydrate(
            entity.ProductId,
            entity.NameNl ?? string.Empty,
            entity.NameEn ?? string.Empty,
            entity.NameFr ?? string.Empty,
            entity.OrderPartNumber,
            entity.SupplierId,
            entity.ManufacturerId,
            entity.ShowOnWebshop == true,
            entity.DescriptionNl,
            entity.DescriptionEn,
            entity.DescriptionFr,
            entity.WebshopDescriptionNl,
            entity.EanCode,
            entity.IsInactive);

    public static void ApplyToEntity(Product domain, PersistenceProduct entity, string? modifiedBy = null)
    {
        entity.NameNl = domain.NameNl;
        entity.NameEn = domain.NameEn;
        entity.NameFr = domain.NameFr;
        entity.DescriptionNl = domain.DescriptionNl ?? string.Empty;
        entity.DescriptionEn = domain.DescriptionEn ?? string.Empty;
        entity.DescriptionFr = domain.DescriptionFr ?? string.Empty;
        entity.OrderPartNumber = domain.OrderPartNumber;
        entity.StockNumber = domain.OrderPartNumber;
        entity.SupplierId = domain.SupplierId;
        entity.ManufacturerId = domain.ManufacturerId;
        entity.ShowOnWebshop = domain.ShowOnWebshop;
        entity.WebshopDescriptionNl = domain.WebshopDescriptionNl ?? string.Empty;
        entity.EanCode = domain.EanCode;
        entity.IsInactive = domain.IsInactive;
        entity.LastModifiedAt = DateTime.UtcNow;
        entity.LastModifiedBy = modifiedBy ?? entity.LastModifiedBy ?? "system";
    }
}
