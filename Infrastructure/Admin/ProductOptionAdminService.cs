using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.ProductOptions;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class ProductOptionAdminService : IProductOptionAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public ProductOptionAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductOptionDto>> GetProductOptionsAsync(ProductOptionListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.ProductOptions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.NameEn.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.NameEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProductOptionDto
            {
                Id = e.Id,
                ProductId = e.ProductId,
                NameEn = e.NameEn,
                ValueType = e.ValueType,
                Tag = e.Tag,
                IsRequired = e.IsRequired,
                CalculatePrice = e.CalculatePrice,
                SortOrder = e.SortOrder
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductOptionDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ProductOptionEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.ProductOptions.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ProductOptionEditDto
            {
                Id = e.Id,
                ProductId = e.ProductId,
                NameEn = e.NameEn,
                ValueType = e.ValueType,
                Tag = e.Tag,
                IsRequired = e.IsRequired,
                CalculatePrice = e.CalculatePrice,
                SortOrder = e.SortOrder
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(ProductOptionEditDto dto, CancellationToken cancellationToken = default)
    {
        ProductOption entity;
        if (dto.Id == 0)
        {
            entity = (ProductOption)AdminCrudDefaults.Create("product-options");
            _db.ProductOptions.Add(entity);
        }
        else
        {
            entity = await _db.ProductOptions.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.ProductId = dto.ProductId;
        entity.NameEn = dto.NameEn;
        entity.Name = dto.NameEn;
        entity.NameFr = dto.NameEn;
        entity.ValueType = dto.ValueType;
        entity.Tag = dto.Tag;
        entity.IsRequired = dto.IsRequired;
        entity.CalculatePrice = dto.CalculatePrice;
        entity.SortOrder = dto.SortOrder;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductOptions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.ProductOptions.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
