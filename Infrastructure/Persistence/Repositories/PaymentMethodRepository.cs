using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.PaymentMethods;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly WebShopABMATICDbContext _db;

    public PaymentMethodRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<PaymentMethodDto>> GetPaymentMethodsAsync(PaymentMethodListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.PaymentMethods.AsNoTracking();

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
            .Select(e => new PaymentMethodDto
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameNl = e.NameNl,
                NameFr = e.NameFr,
                IsPrePay = e.IsPrePay,
                IsPostPay = e.IsPostPay
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<PaymentMethodDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<PaymentMethodEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.PaymentMethods.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new PaymentMethodEditDto
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameNl = e.NameNl,
                NameFr = e.NameFr,
                IsPrePay = e.IsPrePay,
                IsPostPay = e.IsPostPay
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(PaymentMethodEditDto dto, CancellationToken cancellationToken = default)
    {
        PaymentMethod entity;
        if (dto.Id == 0)
        {
            entity = (PaymentMethod)AdminCrudDefaults.Create("payment-methods");
            _db.PaymentMethods.Add(entity);
        }
        else
        {
            entity = await _db.PaymentMethods.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.NameEn = dto.NameEn;
        entity.NameNl = dto.NameNl;
        entity.NameFr = dto.NameFr;
        entity.IsPrePay = dto.IsPrePay;
        entity.IsPostPay = dto.IsPostPay;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.PaymentMethods.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.PaymentMethods.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
