using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductQuantityTiers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductQuantityTierRepository : IProductQuantityTierRepository
{
    private readonly WebShopABMATICDbContext _db;

    public ProductQuantityTierRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductQuantityTierDto>> GetProductQuantityTiersAsync(ProductQuantityTierListFilter filter, CancellationToken cancellationToken = default)
    {
        var query =
            from tier in _db.ProductQuantityTiers.AsNoTracking()
            join product in _db.Products.AsNoTracking() on tier.ProductId equals product.ProductId into productJoin
            from product in productJoin.DefaultIfEmpty()
            select new { tier, product };

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            if (int.TryParse(term, out var productId))
            {
                query = query.Where(x =>
                    x.tier.ProductId == productId ||
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
            .OrderBy(x => x.tier.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductQuantityTierDto
            {
                Id = x.tier.Id,
                ProductId = x.tier.ProductId,
                ProductName = x.product != null ? (x.product.NameEn ?? string.Empty) : string.Empty,
                OrderPartNumber = x.product != null ? x.product.OrderPartNumber : null,
                MinimumQuantity = x.tier.MinimumQuantity,
                Discount = x.tier.Discount
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductQuantityTierDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ProductQuantityTierEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.ProductQuantityTiers.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ProductQuantityTierEditDto
            {
                Id = e.Id,
                ProductId = e.ProductId,
                MinimumQuantity = e.MinimumQuantity,
                Discount = e.Discount
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(ProductQuantityTierEditDto dto, CancellationToken cancellationToken = default)
    {
        await AdminProductExists.EnsureAsync(_db, dto.ProductId, cancellationToken);

        ProductQuantityTier entity;
        if (dto.Id == 0)
        {
            entity = (ProductQuantityTier)AdminCrudDefaults.Create("product-tiers");
            _db.ProductQuantityTiers.Add(entity);
        }
        else
        {
            entity = await _db.ProductQuantityTiers.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.ProductId = dto.ProductId;
        entity.MinimumQuantity = dto.MinimumQuantity;
        entity.Discount = dto.Discount;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductQuantityTiers.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.ProductQuantityTiers.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
