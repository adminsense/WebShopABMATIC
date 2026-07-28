using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductAttributes;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductAttributeAssignmentRepository : IProductAttributeAssignmentRepository
{
    private readonly WebShopABMATICDbContext _db;

    public ProductAttributeAssignmentRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductAttributeAssignmentProductDto>> SearchProductsAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Products.AsNoTracking().Where(p => !p.IsInactive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p =>
                (p.NameNl != null && p.NameNl.Contains(term)) ||
                (p.NameEn != null && p.NameEn.Contains(term)) ||
                (p.NameFr != null && p.NameFr.Contains(term)) ||
                (p.OrderPartNumber != null && p.OrderPartNumber.Contains(term)) ||
                (p.EanCode != null && p.EanCode.Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);
        var safePage = Math.Max(1, page);
        var safePageSize = Math.Clamp(pageSize, 1, 100);

        var items = await query
            .OrderBy(p => p.NameEn)
            .ThenBy(p => p.ProductId)
            .Skip((safePage - 1) * safePageSize)
            .Take(safePageSize)
            .Select(p => new ProductAttributeAssignmentProductDto
            {
                ProductId = p.ProductId,
                NameNl = p.NameNl ?? string.Empty,
                NameEn = p.NameEn ?? string.Empty,
                NameFr = p.NameFr ?? string.Empty,
                OrderPartNumber = p.OrderPartNumber,
                EanCode = p.EanCode
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductAttributeAssignmentProductDto>
        {
            Items = items,
            TotalCount = total,
            Page = safePage,
            PageSize = safePageSize
        };
    }

    public async Task<ProductAttributeAssignmentDto?> GetAssignmentAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _db.Products.AsNoTracking()
            .Where(p => p.ProductId == productId && !p.IsInactive)
            .Select(p => new ProductAttributeAssignmentProductDto
            {
                ProductId = p.ProductId,
                NameNl = p.NameNl ?? string.Empty,
                NameEn = p.NameEn ?? string.Empty,
                NameFr = p.NameFr ?? string.Empty,
                OrderPartNumber = p.OrderPartNumber,
                EanCode = p.EanCode
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return null;
        }

        var values = await (
            from item in _db.ProductAttributeValues.AsNoTracking()
            join attr in _db.ProductAttributes.AsNoTracking() on item.ProductAttributeId equals attr.Id
            where item.ProductId == productId
            orderby attr.SortOrder, attr.NameEn
            select new ProductAttributeValueDto
            {
                Id = item.Id,
                ProductAttributeId = item.ProductAttributeId,
                AttributeNameEn = attr.NameEn,
                AttributeNameNl = attr.NameNl,
                Value = item.Value
            }).ToListAsync(cancellationToken);

        var assignedIds = values.Select(v => v.ProductAttributeId).ToHashSet();
        var available = await _db.ProductAttributes.AsNoTracking()
            .Where(a => !assignedIds.Contains(a.Id))
            .OrderBy(a => a.SortOrder)
            .ThenBy(a => a.NameEn)
            .Select(a => new ProductAttributeDto
            {
                Id = a.Id,
                NameEn = a.NameEn,
                NameNl = a.NameNl,
                NameFr = a.NameFr,
                SortOrder = a.SortOrder
            })
            .ToListAsync(cancellationToken);

        return new ProductAttributeAssignmentDto
        {
            Product = product,
            Values = values,
            AvailableAttributes = available
        };
    }

    public async Task<int> SaveValueAsync(ProductAttributeValueEditDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ProductId <= 0 || dto.ProductAttributeId <= 0)
        {
            throw new InvalidOperationException("Product and attribute are required.");
        }

        var value = (dto.Value ?? string.Empty).Trim();
        if (value.Length == 0)
        {
            throw new InvalidOperationException("Value is required.");
        }

        ProductAttributeValue entity;
        if (dto.Id > 0)
        {
            entity = await _db.ProductAttributeValues.FirstAsync(e => e.Id == dto.Id, cancellationToken);
            entity.Value = value;
        }
        else
        {
            var exists = await _db.ProductAttributeValues.AnyAsync(
                e => e.ProductId == dto.ProductId && e.ProductAttributeId == dto.ProductAttributeId,
                cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException("This attribute is already assigned to the product.");
            }

            entity = new ProductAttributeValue
            {
                ProductId = dto.ProductId,
                ProductAttributeId = dto.ProductAttributeId,
                Value = value
            };
            _db.ProductAttributeValues.Add(entity);
        }

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteValueAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductAttributeValues.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.ProductAttributeValues.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
