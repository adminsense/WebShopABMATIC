using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Domain.Catalog.Products;
using PersistenceProduct = WebShopABMATIC.Data.Entities.Product;

namespace WebShopABMATIC.Infrastructure.Persistence.Mappers;

internal static class ProductPersistenceMapper
{
    public static Product ToDomain(PersistenceProduct entity) =>
        Product.Rehydrate(
            entity.ProductId,
            entity.NameEn ?? string.Empty,
            entity.OrderPartNumber,
            entity.SupplierId,
            entity.ManufacturerId,
            entity.ShowOnWebshop == true,
            entity.WebshopDescriptionNl,
            entity.EanCode,
            entity.IsInactive);

    public static void ApplyToEntity(Product domain, PersistenceProduct entity)
    {
        entity.NameEn = domain.NameEn;
        entity.NameNl = domain.NameEn;
        entity.NameFr = domain.NameEn;
        entity.DescriptionEn = domain.WebshopDescriptionNl;
        entity.DescriptionNl = domain.WebshopDescriptionNl;
        entity.DescriptionFr = domain.WebshopDescriptionNl;
        entity.OrderPartNumber = domain.OrderPartNumber;
        entity.StockNumber = domain.OrderPartNumber;
        entity.SupplierId = domain.SupplierId;
        entity.ManufacturerId = domain.ManufacturerId;
        entity.ShowOnWebshop = domain.ShowOnWebshop;
        entity.WebshopDescriptionNl = domain.WebshopDescriptionNl;
        entity.EanCode = domain.EanCode;
        entity.IsInactive = domain.IsInactive;
        entity.LastModifiedAt = DateTime.UtcNow;
        entity.LastModifiedBy = "admin";
    }
}
