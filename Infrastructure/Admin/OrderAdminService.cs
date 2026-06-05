using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class OrderAdminService : IOrderAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public OrderAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Orders.AsNoTracking();

        if (filter.IsAccepted is bool accepted)
        {
            query = query.Where(o => o.IsAccepted == accepted);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search) && int.TryParse(filter.Search.Trim(), out var orderId))
        {
            query = query.Where(o => o.Id == orderId);
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                ProjectId = o.ProjectId,
                DeliveryTypeId = o.DeliveryTypeId,
                IsAccepted = o.IsAccepted,
                GeneralDiscount = o.GeneralDiscount,
                IsUrgent = o.IsUrgent
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<OrderSummaryDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<OrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        await _db.Orders.AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderEditDto
            {
                Id = o.Id,
                ProjectId = o.ProjectId,
                CreatedAt = o.CreatedAt,
                DeliveryTypeId = o.DeliveryTypeId,
                IsAccepted = o.IsAccepted,
                GeneralDiscount = o.GeneralDiscount,
                CustomerNotes = o.CustomerNotes,
                IsUrgent = o.IsUrgent
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(OrderEditDto dto, CancellationToken cancellationToken = default)
    {
        Order entity;
        if (dto.Id == 0)
        {
            entity = (Order)AdminCrudDefaults.Create("orders");
            _db.Orders.Add(entity);
        }
        else
        {
            entity = await _db.Orders.FirstAsync(o => o.Id == dto.Id, cancellationToken);
        }

        entity.ProjectId = dto.ProjectId;
        entity.CreatedAt = dto.CreatedAt;
        entity.DeliveryTypeId = dto.DeliveryTypeId;
        entity.IsAccepted = dto.IsAccepted;
        entity.GeneralDiscount = dto.GeneralDiscount;
        entity.CustomerNotes = dto.CustomerNotes;
        entity.IsUrgent = dto.IsUrgent;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
