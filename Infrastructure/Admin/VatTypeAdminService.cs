using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.VatTypes;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class VatTypeAdminService : IVatTypeAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public VatTypeAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<VatTypeDto>> GetVatTypesAsync(VatTypeListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.VatTypes.AsNoTracking();

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
            .Select(e => new VatTypeDto
            {
                Id = e.Id,
                Name = e.Name,
                Percentage = e.Percentage,
                InvoiceText = e.InvoiceText,
                InvoiceTextEn = e.InvoiceTextEn,
                InvoiceTextFr = e.InvoiceTextFr,
                IsDefault = e.IsDefault
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<VatTypeDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<VatTypeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.VatTypes.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new VatTypeEditDto
            {
                Id = e.Id,
                Name = e.Name,
                Percentage = e.Percentage,
                InvoiceText = e.InvoiceText,
                InvoiceTextEn = e.InvoiceTextEn,
                InvoiceTextFr = e.InvoiceTextFr,
                IsDefault = e.IsDefault
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(VatTypeEditDto dto, CancellationToken cancellationToken = default)
    {
        VatType entity;
        var isNew = dto.Id == 0;
        if (isNew)
        {
            entity = (VatType)AdminCrudDefaults.Create("vat-types");
            _db.VatTypes.Add(entity);
        }
        else
        {
            entity = await _db.VatTypes.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.Percentage = dto.Percentage;
        entity.InvoiceText = dto.InvoiceText;
        entity.InvoiceTextEn = dto.InvoiceTextEn;
        entity.InvoiceTextFr = dto.InvoiceTextFr;
        entity.IsDefault = dto.IsDefault;

        if (isNew)
        {
            entity.ExplanationNl = dto.InvoiceText;
            entity.ExplanationEn = dto.InvoiceTextEn;
            entity.ExplanationFr = dto.InvoiceTextFr;
        }

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.VatTypes.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.VatTypes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
