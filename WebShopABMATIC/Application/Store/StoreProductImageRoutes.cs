namespace WebShopABMATIC.Application.Store;

public static class StoreProductImageRoutes
{
    public static string PrimaryImage(int productId) => $"/api/store/products/{productId}/image";
}
