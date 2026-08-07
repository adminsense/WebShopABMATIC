using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence;

internal static class AdminProductExists
{
    public static async Task EnsureAsync(WebShopABMATICDbContext db, int productId, CancellationToken cancellationToken = default)
    {
        if (productId <= 0)
        {
            throw new InvalidOperationException("Select a product before saving.");
        }

        var exists = await db.Products.AsNoTracking()
            .AnyAsync(p => p.ProductId == productId, cancellationToken);
        if (!exists)
        {
            throw new InvalidOperationException($"Product #{productId} was not found.");
        }
    }
}
