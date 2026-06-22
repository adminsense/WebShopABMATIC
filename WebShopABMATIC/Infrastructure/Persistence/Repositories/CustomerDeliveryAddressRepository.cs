using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.CustomerDeliveryAddresses;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class CustomerDeliveryAddressRepository : ICustomerDeliveryAddressRepository
{
    private readonly WebShopABMATICDbContext _db;

    public CustomerDeliveryAddressRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<CustomerDeliveryAddressDto>> GetCustomerDeliveryAddressesAsync(CustomerDeliveryAddressListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.CustomerDeliveryAddresses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Name.Contains(term) || e.Straat.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new CustomerDeliveryAddressDto
            {
                Id = e.Id,
                CustomerId = e.CustomerId,
                Name = e.Name,
                Straat = e.Straat,
                Number = e.Number,
                Bus = e.Bus,
                CityId = e.CityId
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerDeliveryAddressDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<CustomerDeliveryAddressEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.CustomerDeliveryAddresses.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new CustomerDeliveryAddressEditDto
            {
                Id = e.Id,
                CustomerId = e.CustomerId,
                Name = e.Name,
                Straat = e.Straat,
                Number = e.Number,
                Bus = e.Bus,
                CityId = e.CityId
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(CustomerDeliveryAddressEditDto dto, CancellationToken cancellationToken = default)
    {
        CustomerDeliveryAddress entity;
        if (dto.Id == 0)
        {
            entity = (CustomerDeliveryAddress)AdminCrudDefaults.Create("delivery-addresses");
            _db.CustomerDeliveryAddresses.Add(entity);
        }
        else
        {
            entity = await _db.CustomerDeliveryAddresses.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.CustomerId = dto.CustomerId;
        entity.Name = dto.Name;
        entity.Straat = dto.Straat;
        entity.Number = dto.Number;
        entity.Bus = dto.Bus;
        entity.CityId = dto.CityId;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CustomerDeliveryAddresses.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.CustomerDeliveryAddresses.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
