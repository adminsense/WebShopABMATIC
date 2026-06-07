using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.WebshopStructures;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class WebshopStructureRepository : IWebshopStructureRepository
{
    private readonly WebShopABMATICDbContext _db;

    public WebshopStructureRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<WebshopStructureDto>> GetWebshopStructuresAsync(WebshopStructureListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.WebshopStructures.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.NameNl.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.NameNl)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new WebshopStructureDto
            {
                Id = e.Id,
                NameNl = e.NameNl,
                ParentTaskId = e.ParentTaskId,
                SortOrder = e.SortOrder
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<WebshopStructureDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<WebshopStructureEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.WebshopStructures.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new WebshopStructureEditDto
            {
                Id = e.Id,
                NameNl = e.NameNl,
                ParentTaskId = e.ParentTaskId,
                SortOrder = e.SortOrder
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(WebshopStructureEditDto dto, CancellationToken cancellationToken = default)
    {
        WebshopStructure entity;
        if (dto.Id == 0)
        {
            entity = (WebshopStructure)AdminCrudDefaults.Create("webshop-structures");
            _db.WebshopStructures.Add(entity);
        }
        else
        {
            entity = await _db.WebshopStructures.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.NameNl = dto.NameNl;
        entity.ParentTaskId = dto.ParentTaskId;
        entity.SortOrder = dto.SortOrder;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.WebshopStructures.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.WebshopStructures.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
