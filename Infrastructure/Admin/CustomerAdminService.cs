using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class CustomerAdminService : ICustomerAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public CustomerAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Customers.AsNoTracking().AsQueryable();

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
                CustomerEmail = c.CustomerEmail
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
