using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.DeliveryTypes;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class DeliveryTypeAdminService : IDeliveryTypeAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public DeliveryTypeAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<DeliveryTypeDto>> GetDeliveryTypesAsync(DeliveryTypeListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.DeliveryTypes.AsNoTracking();

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
            .Select(e => new DeliveryTypeDto
            {
                Id = e.Id,
                Name = e.Name,
                NameFr = e.NameFr,
                IncludeInstallationCost = e.IncludeInstallationCost,
                IsDefault = e.IsDefault
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<DeliveryTypeDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<DeliveryTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.DeliveryTypes.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new DeliveryTypeEditDto
            {
                Id = e.Id,
                Name = e.Name,
                NameFr = e.NameFr,
                IncludeInstallationCost = e.IncludeInstallationCost,
                IsDefault = e.IsDefault
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(DeliveryTypeEditDto dto, CancellationToken cancellationToken = default)
    {
        DeliveryType entity;
        if (dto.Id == 0)
        {
            entity = (DeliveryType)AdminCrudDefaults.Create("delivery-types");
            _db.DeliveryTypes.Add(entity);
        }
        else
        {
            entity = await _db.DeliveryTypes.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.NameFr = dto.NameFr;
        entity.IncludeInstallationCost = dto.IncludeInstallationCost;
        entity.IsDefault = dto.IsDefault;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.DeliveryTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.DeliveryTypes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
