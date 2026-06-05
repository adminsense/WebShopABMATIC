using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Suppliers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class SupplierAdminService : ISupplierAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public SupplierAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<SupplierDto>> GetSuppliersAsync(SupplierListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Suppliers.AsNoTracking().Where(e => !e.IsInactive);

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
            .Select(e => new SupplierDto
            {
                SupplierId = e.SupplierId,
                Name = e.Name,
                CityId = e.CityId,
                LanguageId = e.LanguageId,
                Email = e.Email,
                GeneralLedgerRevenueAccount = e.GeneralLedgerRevenueAccount,
                IsInactive = e.IsInactive
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<SupplierDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<SupplierEditDto?> GetForEditAsync(int supplierId, CancellationToken cancellationToken = default) =>
        await _db.Suppliers.AsNoTracking()
            .Where(e => e.SupplierId == supplierId)
            .Select(e => new SupplierEditDto
            {
                SupplierId = e.SupplierId,
                Name = e.Name,
                CityId = e.CityId,
                LanguageId = e.LanguageId,
                Email = e.Email,
                GeneralLedgerRevenueAccount = e.GeneralLedgerRevenueAccount,
                IsInactive = e.IsInactive
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(SupplierEditDto dto, CancellationToken cancellationToken = default)
    {
        Supplier entity;
        if (dto.SupplierId == 0)
        {
            entity = (Supplier)AdminCrudDefaults.Create("suppliers");
            _db.Suppliers.Add(entity);
        }
        else
        {
            entity = await _db.Suppliers.FirstAsync(e => e.SupplierId == dto.SupplierId, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.CityId = dto.CityId;
        entity.LanguageId = dto.LanguageId;
        entity.Email = dto.Email;
        entity.GeneralLedgerRevenueAccount = dto.GeneralLedgerRevenueAccount;
        entity.IsInactive = dto.IsInactive;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.SupplierId;
    }

    public async Task<bool> DeleteAsync(int supplierId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Suppliers.FirstOrDefaultAsync(e => e.SupplierId == supplierId, cancellationToken);
        if (entity is null) return false;
        entity.IsInactive = true;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
