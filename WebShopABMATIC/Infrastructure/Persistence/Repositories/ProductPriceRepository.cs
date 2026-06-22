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
        var query = _db.ProductPrices.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search) && int.TryParse(filter.Search.Trim(), out var productId))
        {
            query = query.Where(e => e.ProductId == productId);
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProductPriceDto
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
