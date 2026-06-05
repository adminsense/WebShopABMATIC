using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.CustomerTypes;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class CustomerTypeAdminService : ICustomerTypeAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public CustomerTypeAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<CustomerTypeDto>> GetCustomerTypesAsync(CustomerTypeListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.CustomerTypes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e =>
                e.CustomerTypeName.Contains(term) ||
                e.CustomerTypeNameFr.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.CustomerTypeName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new CustomerTypeDto
            {
                KlantTypeId = e.KlantTypeId,
                CustomerTypeName = e.CustomerTypeName,
                CustomerTypeNameFr = e.CustomerTypeNameFr,
                BaseDiscount = e.BaseDiscount,
                SortOrder = e.SortOrder,
                PaymentTermId = e.PaymentTermId,
                VatSystemId = e.VatSystemId,
                DeliveryTypeId = e.DeliveryTypeId,
                RequiresVatNumber = e.RequiresVatNumber,
                IsDefault = e.IsDefault
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerTypeDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<CustomerTypeEditDto?> GetForEditAsync(int klantTypeId, CancellationToken cancellationToken = default) =>
        await _db.CustomerTypes.AsNoTracking()
            .Where(e => e.KlantTypeId == klantTypeId)
            .Select(e => new CustomerTypeEditDto
            {
                KlantTypeId = e.KlantTypeId,
                CustomerTypeName = e.CustomerTypeName,
                CustomerTypeNameFr = e.CustomerTypeNameFr,
                BaseDiscount = e.BaseDiscount,
                SortOrder = e.SortOrder,
                PaymentTermId = e.PaymentTermId,
                VatSystemId = e.VatSystemId,
                DeliveryTypeId = e.DeliveryTypeId,
                RequiresVatNumber = e.RequiresVatNumber,
                IsDefault = e.IsDefault
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(CustomerTypeEditDto dto, CancellationToken cancellationToken = default)
    {
        CustomerType entity;
        if (dto.KlantTypeId == 0)
        {
            entity = (CustomerType)AdminCrudDefaults.Create("customer-types");
            _db.CustomerTypes.Add(entity);
        }
        else
        {
            entity = await _db.CustomerTypes.FirstAsync(e => e.KlantTypeId == dto.KlantTypeId, cancellationToken);
        }

        entity.CustomerTypeName = dto.CustomerTypeName;
        entity.CustomerTypeNameFr = dto.CustomerTypeNameFr;
        entity.BaseDiscount = dto.BaseDiscount;
        entity.SortOrder = dto.SortOrder;
        entity.PaymentTermId = dto.PaymentTermId;
        entity.VatSystemId = dto.VatSystemId;
        entity.DeliveryTypeId = dto.DeliveryTypeId;
        entity.RequiresVatNumber = dto.RequiresVatNumber;
        entity.IsDefault = dto.IsDefault;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.KlantTypeId;
    }

    public async Task<bool> DeleteAsync(int klantTypeId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.CustomerTypes.FirstOrDefaultAsync(e => e.KlantTypeId == klantTypeId, cancellationToken);
        if (entity is null) return false;
        _db.CustomerTypes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
