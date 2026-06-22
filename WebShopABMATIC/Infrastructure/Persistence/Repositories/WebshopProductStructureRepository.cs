using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.WebshopProductStructures;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class WebshopProductStructureRepository : IWebshopProductStructureRepository
{
    private readonly WebShopABMATICDbContext _db;

    public WebshopProductStructureRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<WebshopProductStructureDto>> GetWebshopProductStructuresAsync(WebshopProductStructureListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.WebshopProductStructures.AsNoTracking();

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
            .OrderBy(e => e.NameEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new WebshopProductStructureDto
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameNl = e.NameNl,
                NameFr = e.NameFr,
                ParentTaskId = e.ParentTaskId
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<WebshopProductStructureDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<WebshopProductStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.WebshopProductStructures.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new WebshopProductStructureEditDto
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameNl = e.NameNl,
                NameFr = e.NameFr,
                ParentTaskId = e.ParentTaskId
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(WebshopProductStructureEditDto dto, CancellationToken cancellationToken = default)
    {
        WebshopProductStructure entity;
        if (dto.Id == 0)
        {
            entity = (WebshopProductStructure)AdminCrudDefaults.Create("webshop-product-structures");
            _db.WebshopProductStructures.Add(entity);
        }
        else
        {
            entity = await _db.WebshopProductStructures.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.NameEn = dto.NameEn;
        entity.NameNl = dto.NameNl;
        entity.NameFr = dto.NameFr;
        entity.ParentTaskId = dto.ParentTaskId;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.WebshopProductStructures.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.WebshopProductStructures.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
