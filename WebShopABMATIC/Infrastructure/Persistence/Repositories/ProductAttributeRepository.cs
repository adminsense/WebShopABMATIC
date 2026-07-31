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
                e.Name.Contains(term) ||
                (e.DataType != null && e.DataType.Contains(term)) ||
                (e.Unit != null && e.Unit.Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .ThenBy(e => e.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProductAttributeDto
            {
                Id = e.Id,
                Name = e.Name,
                DataType = e.DataType,
                Unit = e.Unit
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
                Name = e.Name,
                DataType = e.DataType,
                Unit = e.Unit
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

        entity.Name = dto.Name.Trim();
        entity.DataType = string.IsNullOrWhiteSpace(dto.DataType) ? null : dto.DataType.Trim();
        entity.Unit = string.IsNullOrWhiteSpace(dto.Unit) ? null : dto.Unit.Trim();

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
