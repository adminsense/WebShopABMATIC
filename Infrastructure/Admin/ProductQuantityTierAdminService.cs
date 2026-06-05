using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductQuantityTiers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class ProductQuantityTierAdminService : IProductQuantityTierAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public ProductQuantityTierAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductQuantityTierDto>> GetProductQuantityTiersAsync(ProductQuantityTierListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.ProductQuantityTiers.AsNoTracking();

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
            .Select(e => new ProductQuantityTierDto
            {
                Id = e.Id,
                ProductId = e.ProductId,
                MinimumQuantity = e.MinimumQuantity,
                Discount = e.Discount
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
