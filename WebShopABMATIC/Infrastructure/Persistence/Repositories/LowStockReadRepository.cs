using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class LowStockReadRepository : ILowStockReadRepository
{
    private readonly WebShopABMATICDbContext _db;

    public LowStockReadRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<IReadOnlyList<LowStockProductDto>> GetLowStockProductsAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        var take = Math.Clamp(limit, 1, 100);

        return await (
            from psl in _db.ProductStockLocations.AsNoTracking()
            join product in _db.Products.AsNoTracking() on psl.ProductId equals product.ProductId
            join location in _db.StockLocations.AsNoTracking() on psl.StockLocationId equals location.Id
            where psl.IsInactive != true && psl.Quantity <= psl.MinQuantity
            orderby psl.Quantity, product.NameEn
            select new LowStockProductDto
            {
                ProductStockLocationId = psl.Id,
                ProductId = psl.ProductId,
                ProductName = product.NameEn,
                StockLocationId = psl.StockLocationId,
                StockLocationName = location.Name,
                Quantity = psl.Quantity,
                MinQuantity = psl.MinQuantity
            }).Take(take).ToListAsync(cancellationToken);
    }
}
