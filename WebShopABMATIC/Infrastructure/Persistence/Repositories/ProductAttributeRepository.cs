using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductAttributes;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductAttributeRepository : IProductAttributeRepository
{
    private readonly WebShopABMATICDbContext _db;

    public ProductAttributeRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductAttributeDto>> GetAttributesAsync(ProductAttributeListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.ProductAttributes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e =>
                e.NameEn.Contains(term) ||
                e.NameNl.Contains(term) ||
                e.NameFr.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.SortOrder)
            .ThenBy(e => e.NameEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProductAttributeDto
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameNl = e.NameNl,
                NameFr = e.NameFr,
                SortOrder = e.SortOrder
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductAttributeDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ProductAttributeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.ProductAttributes.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ProductAttributeEditDto
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameNl = e.NameNl,
                NameFr = e.NameFr,
                SortOrder = e.SortOrder
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(ProductAttributeEditDto dto, CancellationToken cancellationToken = default)
    {
        ProductAttribute entity;
        if (dto.Id == 0)
        {
            entity = (ProductAttribute)AdminCrudDefaults.Create("attributes");
            _db.ProductAttributes.Add(entity);
        }
        else
        {
            entity = await _db.ProductAttributes.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.NameEn = dto.NameEn.Trim();
        entity.NameNl = dto.NameNl.Trim();
        entity.NameFr = dto.NameFr.Trim();
        entity.SortOrder = dto.SortOrder;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductAttributes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.ProductAttributes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<bool> HasValuesAsync(int attributeId, CancellationToken cancellationToken = default) =>
        _db.ProductAttributeValues.AsNoTracking().AnyAsync(v => v.ProductAttributeId == attributeId, cancellationToken);
}
