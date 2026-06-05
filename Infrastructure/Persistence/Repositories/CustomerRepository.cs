using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly WebShopABMATICDbContext _db;

    public CustomerRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(c =>
                c.CustomerName.Contains(term) ||
                c.CustomerEmail.Contains(term) ||
                (c.WebshopLogin != null && c.WebshopLogin.Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(c => c.CustomerName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                WebshopLogin = c.WebshopLogin,
                CustomerTypeId = c.CustomerTypeId,
                CustomerEmail = c.CustomerEmail,
                CustomerPhone = c.CustomerPhone,
                CustomerCityId = c.CustomerCityId,
                Locked = c.Locked
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<CustomerEditDto?> GetForEditAsync(int customerId, CancellationToken cancellationToken = default) =>
        await _db.Customers.AsNoTracking()
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerEditDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                WebshopLogin = c.WebshopLogin,
                CustomerEmail = c.CustomerEmail,
                CustomerPhone = c.CustomerPhone,
                CustomerTypeId = c.CustomerTypeId,
                CustomerCityId = c.CustomerCityId,
                CustomerStreet = c.CustomerStreet,
                CustomerHouseNumber = c.CustomerHouseNumber,
                CustomerBox = c.CustomerBox,
                CustomerVatNumber = c.CustomerVatNumber,
                DeliveryTypeId = c.DeliveryTypeId,
                BetaaltermijnId = c.BetaaltermijnId,
                Locked = c.Locked
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(CustomerEditDto dto, CancellationToken cancellationToken = default)
    {
        Customer entity;
        if (dto.CustomerId == 0)
        {
            entity = (Customer)AdminCrudDefaults.Create("customers");
            _db.Customers.Add(entity);
        }
        else
        {
            entity = await _db.Customers.FirstAsync(c => c.CustomerId == dto.CustomerId, cancellationToken);
        }

        entity.CustomerName = dto.CustomerName;
        entity.WebshopLogin = dto.WebshopLogin;
        entity.CustomerEmail = dto.CustomerEmail;
        entity.CustomerPhone = dto.CustomerPhone;
        entity.CustomerTypeId = dto.CustomerTypeId;
        entity.CustomerCityId = dto.CustomerCityId;
        entity.CustomerStreet = dto.CustomerStreet;
        entity.CustomerHouseNumber = dto.CustomerHouseNumber;
        entity.CustomerBox = dto.CustomerBox;
        entity.CustomerVatNumber = dto.CustomerVatNumber;
        entity.DeliveryTypeId = dto.DeliveryTypeId;
        entity.BetaaltermijnId = dto.BetaaltermijnId;
        entity.Locked = dto.Locked;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.CustomerId;
    }

    public async Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        if (entity is null) return false;
        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
