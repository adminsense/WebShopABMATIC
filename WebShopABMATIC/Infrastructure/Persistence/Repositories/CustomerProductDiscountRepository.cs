using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.CustomerProductDiscounts;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class CustomerProductDiscountRepository : ICustomerProductDiscountRepository
{
    private readonly WebShopABMATICDbContext _db;

    public CustomerProductDiscountRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<CustomerProductDiscountDto>> GetCustomerProductDiscountsAsync(CustomerProductDiscountListFilter filter, CancellationToken cancellationToken = default)
    {
        var query =
            from d in _db.CustomerProductDiscounts.AsNoTracking()
            join c in _db.Customers.AsNoTracking() on d.CustomerId equals c.CustomerId into customerJoin
            from c in customerJoin.DefaultIfEmpty()
            join p in _db.Products.AsNoTracking() on d.ProductId equals p.ProductId into productJoin
            from p in productJoin.DefaultIfEmpty()
            select new { d, c, p };

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(x =>
                (x.c != null && x.c.CustomerName.Contains(term)) ||
                (x.p != null && x.p.NameEn != null && x.p.NameEn.Contains(term)) ||
                (x.p != null && x.p.OrderPartNumber != null && x.p.OrderPartNumber.Contains(term)) ||
                (x.d.Notes != null && x.d.Notes.Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(x => x.d.CustomerId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CustomerProductDiscountDto
            {
                Id = x.d.Id,
                CustomerId = x.d.CustomerId,
                ProductId = x.d.ProductId,
                DiscountPercentage = x.d.DiscountPercentage,
                FromAddress = x.d.FromAddress,
                ValidTo = x.d.ValidTo,
                Notes = x.d.Notes
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerProductDiscountDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<CustomerProductDiscountEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.CustomerProductDiscounts.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new CustomerProductDiscountEditDto
            {
                Id = e.Id,
                CustomerId = e.CustomerId,
                ProductId = e.ProductId,
                DiscountPercentage = e.DiscountPercentage,
                FromAddress = e.FromAddress,
                ValidTo = e.ValidTo,
                Notes = e.Notes
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(CustomerProductDiscountEditDto dto, CancellationToken cancellationToken = default)
    {
        CustomerProductDiscount entity;
        if (dto.Id == 0)
        {
            entity = (CustomerProductDiscount)AdminCrudDefaults.Create("customer-discounts");
            entity.UserId = Math.Max(1, entity.UserId);
            entity.CreatedAt = DateTime.UtcNow;
            _db.CustomerProductDiscounts.Add(entity);
        }
        else
        {
            entity = await _db.CustomerProductDiscounts.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.CustomerId = dto.CustomerId;
        entity.ProductId = dto.ProductId;
        entity.DiscountPercentage = dto.DiscountPercentage;
        entity.FromAddress = dto.FromAddress;
        entity.ValidTo = dto.ValidTo;
        entity.Notes = dto.Notes;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CustomerProductDiscounts.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.CustomerProductDiscounts.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
