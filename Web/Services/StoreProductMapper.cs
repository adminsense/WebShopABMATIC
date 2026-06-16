using WebShopABMATIC.Application.Store;

namespace WebShopABMATIC.Web.Services;

public static class StoreProductMapper
{
    public static StoreProduct ToModel(StoreProductDto dto) =>
        new(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.ImageUrl,
            dto.Price,
            dto.Stock,
            dto.MinQuantity,
            dto.IsLowStock,
            dto.IsOutOfStock,
            dto.Category,
            dto.Tag);
}
