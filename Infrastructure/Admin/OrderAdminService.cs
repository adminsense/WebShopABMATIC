using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class OrderAdminService : IOrderAdminPort
{
    private readonly WebShopABMATICDbContext _db;

    public OrderAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default)
    {
        try
        {
        var query = _db.Orders.AsNoTracking().AsQueryable();

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
                IsAccepted = o.IsAccepted
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<OrderSummaryDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
        }
        catch
        {
            return new PagedResult<OrderSummaryDto> { Items = [], TotalCount = 0, Page = 1, PageSize = filter.PageSize };
        }
    }
}
