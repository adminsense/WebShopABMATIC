using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.OrderStatuses;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class OrderStatusAdminService : IOrderStatusAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public OrderStatusAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<OrderStatusDto>> GetOrderStatusesAsync(OrderStatusListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.OrderStatuses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(e => e.Name.Contains(term) || e.NameFr.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new OrderStatusDto
            {
                Id = e.Id,
                Name = e.Name,
                NameFr = e.NameFr,
                ScreenMode = e.ScreenMode,
                SortOrder = e.SortOrder,
                ReserveStock = e.ReserveStock,
                AffectsStock = e.AffectsStock
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<OrderStatusDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<OrderStatusEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.OrderStatuses.AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new OrderStatusEditDto
            {
                Id = e.Id,
                Name = e.Name,
                NameFr = e.NameFr,
                ScreenMode = e.ScreenMode,
                SortOrder = e.SortOrder,
                ReserveStock = e.ReserveStock,
                AffectsStock = e.AffectsStock
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(OrderStatusEditDto dto, CancellationToken cancellationToken = default)
    {
        OrderStatus entity;
        if (dto.Id == 0)
        {
            entity = (OrderStatus)AdminCrudDefaults.Create("order-statuses");
            _db.OrderStatuses.Add(entity);
        }
        else
        {
            entity = await _db.OrderStatuses.FirstAsync(e => e.Id == dto.Id, cancellationToken);
        }

        entity.Name = dto.Name;
        entity.NameFr = dto.NameFr;
        entity.ScreenMode = dto.ScreenMode;
        entity.SortOrder = dto.SortOrder;
        entity.ReserveStock = dto.ReserveStock;
        entity.AffectsStock = dto.AffectsStock;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.OrderStatuses.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return false;
        _db.OrderStatuses.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
