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
        var query = _db.CustomerProductDiscounts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Notes != null && e.Notes.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.CustomerId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new CustomerProductDiscountDto
            {
                Id = e.Id,
                CustomerId = e.CustomerId,
                ProductId = e.ProductId,
                DiscountPercentage = e.DiscountPercentage,
                FromAddress = e.FromAddress,
                ValidTo = e.ValidTo,
                Notes = e.Notes
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
