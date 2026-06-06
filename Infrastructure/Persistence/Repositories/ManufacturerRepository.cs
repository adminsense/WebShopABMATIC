using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Manufacturers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ManufacturerRepository : IManufacturerRepository
{
    private readonly WebShopABMATICDbContext _db;

    public ManufacturerRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ManufacturerDto>> GetManufacturersAsync(ManufacturerListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Manufacturers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Name.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ManufacturerDto
            {
                ManufacturerId = e.ManufacturerId,
                Name = e.Name,
                CityId = e.CityId,
                Email = e.Email,
                Phone = e.Phone,
                Address = e.Address
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ManufacturerDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ManufacturerEditDto?> GetForEditAsync(int manufacturerId, CancellationToken cancellationToken = default) =>
        await _db.Manufacturers.AsNoTracking()
            .Where(e => e.ManufacturerId == manufacturerId)
            .Select(e => new ManufacturerEditDto
            {
                ManufacturerId = e.ManufacturerId,
                Name = e.Name,
                CityId = e.CityId,
                Email = e.Email,
                Phone = e.Phone,
                Address = e.Address
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(ManufacturerEditDto dto, CancellationToken cancellationToken = default)
    {
        Manufacturer entity;
        if (dto.ManufacturerId == 0)
        {
            entity = (Manufacturer)AdminCrudDefaults.Create("manufacturers");
            _db.Manufacturers.Add(entity);
        }
        else
        {
            entity = await _db.Manufacturers.FirstAsync(e => e.ManufacturerId == dto.ManufacturerId, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.CityId = dto.CityId;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.Address = dto.Address;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.ManufacturerId;
    }

    public async Task<bool> DeleteAsync(int manufacturerId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Manufacturers.FirstOrDefaultAsync(e => e.ManufacturerId == manufacturerId, cancellationToken);
        if (entity is null) return false;
        _db.Manufacturers.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
