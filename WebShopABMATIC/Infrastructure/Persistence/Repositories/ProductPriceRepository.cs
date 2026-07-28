using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductPrices;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductPriceRepository : IProductPriceRepository
{
    private readonly WebShopABMATICDbContext _db;

    public ProductPriceRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductPriceDto>> GetProductPricesAsync(ProductPriceListFilter filter, CancellationToken cancellationToken = default)
    {
        var query =
            from price in _db.ProductPrices.AsNoTracking()
            join product in _db.Products.AsNoTracking() on price.ProductId equals product.ProductId into productJoin
            from product in productJoin.DefaultIfEmpty()
            select new { price, product };

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            if (int.TryParse(term, out var productId))
            {
                query = query.Where(x =>
                    x.price.ProductId == productId ||
                    (x.product != null && x.product.NameEn != null && x.product.NameEn.Contains(term)) ||
                    (x.product != null && x.product.OrderPartNumber != null && x.product.OrderPartNumber.Contains(term)));
            }
            else
            {
                query = query.Where(x =>
                    (x.product != null && x.product.NameEn != null && x.product.NameEn.Contains(term)) ||
                    (x.product != null && x.product.OrderPartNumber != null && x.product.OrderPartNumber.Contains(term)));
            }
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(x => x.price.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductPriceDto
            {
                Id = x.price.Id,
                ProductId = x.price.ProductId,
                FromAddress = x.price.FromAddress,
                ValidTo = x.price.ValidTo,
                GrossSalesPrice = x.price.GrossSalesPrice,
                GrossPurchasePrice = x.price.GrossPurchasePrice,
                NetPurchasePrice = x.price.NetPurchasePrice,
                BasePrice = x.price.BasePrice,
                AssemblyPrice = x.price.AssemblyPrice,
                InstallationPrice = x.price.InstallationPrice
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductPriceDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ProductPriceEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.ProductPrices.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ProductPriceEditDto
            {
                Id = e.Id,
                ProductId = e.ProductId,
                FromAddress = e.FromAddress,
                ValidTo = e.ValidTo,
                GrossSalesPrice = e.GrossSalesPrice,
                GrossPurchasePrice = e.GrossPurchasePrice,
                NetPurchasePrice = e.NetPurchasePrice,
                BasePrice = e.BasePrice,
                AssemblyPrice = e.AssemblyPrice,
                InstallationPrice = e.InstallationPrice
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(ProductPriceEditDto dto, CancellationToken cancellationToken = default)
    {
        ProductPrice entity;
        if (dto.Id == 0)
        {
            entity = (ProductPrice)AdminCrudDefaults.Create("product-prices");
            _db.ProductPrices.Add(entity);
        }
        else
        {
            entity = await _db.ProductPrices.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.ProductId = dto.ProductId;
        entity.FromAddress = dto.FromAddress;
        entity.ValidTo = dto.ValidTo;
        entity.GrossSalesPrice = dto.GrossSalesPrice;
        entity.GrossPurchasePrice = dto.GrossPurchasePrice;
        entity.NetPurchasePrice = dto.NetPurchasePrice;
        entity.BasePrice = dto.BasePrice;
        entity.AssemblyPrice = dto.AssemblyPrice;
        entity.InstallationPrice = dto.InstallationPrice;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductPrices.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.ProductPrices.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
