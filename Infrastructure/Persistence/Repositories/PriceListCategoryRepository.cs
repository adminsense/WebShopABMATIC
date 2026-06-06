using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.PriceListCategories;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class PriceListCategoryRepository : IPriceListCategoryRepository
{
    private readonly WebShopABMATICDbContext _db;

    public PriceListCategoryRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<PriceListCategoryDto>> GetPriceListCategoriesAsync(PriceListCategoryListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.PriceListCategories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Name.Contains(term) || e.NameFr.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new PriceListCategoryDto
            {
                Id = e.Id,
                Name = e.Name,
                NameFr = e.NameFr,
                SortOrder = e.SortOrder,
                Color = e.Color,
                HasOptions = e.HasOptions
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<PriceListCategoryDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<PriceListCategoryEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.PriceListCategories.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new PriceListCategoryEditDto
            {
                Id = e.Id,
                Name = e.Name,
                NameFr = e.NameFr,
                SortOrder = e.SortOrder,
                Color = e.Color,
                HasOptions = e.HasOptions
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(PriceListCategoryEditDto dto, CancellationToken cancellationToken = default)
    {
        PriceListCategory entity;
        if (dto.Id == 0)
        {
            entity = (PriceListCategory)AdminCrudDefaults.Create("price-list-categories");
            _db.PriceListCategories.Add(entity);
        }
        else
        {
            entity = await _db.PriceListCategories.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.NameFr = dto.NameFr;
        entity.SortOrder = dto.SortOrder;
        entity.Color = dto.Color;
        entity.HasOptions = dto.HasOptions;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.PriceListCategories.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.PriceListCategories.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
