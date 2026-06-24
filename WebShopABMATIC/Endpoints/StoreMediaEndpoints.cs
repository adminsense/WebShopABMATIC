using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store;

namespace WebShopABMATIC.Web.Endpoints;

public static class StoreMediaEndpoints
{
    public static void MapStoreMediaEndpoints(this WebApplication app)
    {
        app.MapGet("/api/store/products/{productId:int}/image", async (
            int productId,
            IProductMediaPort media,
            CancellationToken cancellationToken) =>
        {
            var url = await media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: true, cancellationToken);
            if (string.IsNullOrWhiteSpace(url))
            {
                return Results.LocalRedirect(FallbackImage(productId));
            }

            return Results.Redirect(url);
        }).DisableAntiforgery();
    }

    private static string FallbackImage(int productId) =>
        productId is >= 1 and <= 6 ? $"/images/product{productId}.png" : "/images/product1.png";
}
