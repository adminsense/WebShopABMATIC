using WebShopABMATIC.Application.Admin.Products;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductMediaPort
{
    Task<string?> GetPrimaryImageUrlAsync(int productId, bool webPublishedOnly = false, CancellationToken cancellationToken = default);
    Task SavePrimaryImageAsync(int productId, ProductImageUpload upload, bool publishToWeb, int createdByUserId, CancellationToken cancellationToken = default);
    Task SetPrimaryImagePublishToWebAsync(int productId, bool publishToWeb, CancellationToken cancellationToken = default);
}
