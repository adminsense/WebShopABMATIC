using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly WebShopABMATICDbContext _db;
    private readonly ICurrentUserContext _currentUser;

    public OrderRepository(WebShopABMATICDbContext db, ICurrentUserContext currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default)
    {
        var query =
            from o in _db.Orders.AsNoTracking()
            join p in _db.Projects.AsNoTracking() on o.ProjectId equals p.ProjectId into projectJoin
            from p in projectJoin.DefaultIfEmpty()
            join c in _db.Customers.AsNoTracking() on p.CustomerId equals c.CustomerId into customerJoin
            from c in customerJoin.DefaultIfEmpty()
            select new { o, CustomerName = c != null ? c.CustomerName : "" };

        if (filter.IsAccepted is bool accepted)
        {
            query = query.Where(x => x.o.IsAccepted == accepted);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            if (int.TryParse(term, out var orderId))
            {
                query = query.Where(x => x.o.Id == orderId || x.o.OrderNumber == orderId);
            }
            else
            {
                query = query.Where(x => x.CustomerName.Contains(term));
            }
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var pageRows = await query
            .OrderByDescending(x => x.o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new { x.o.Id, x.o.OrderNumber, x.o.CreatedAt, x.o.DeliveryTypeId, x.o.IsAccepted, x.o.GeneralDiscount, x.o.IsUrgent, x.CustomerName })
            .ToListAsync(cancellationToken);

        var orderIds = pageRows.Select(x => x.Id).ToList();
        var paymentRows = await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => orderIds.Contains(a.OrderId))
            .OrderBy(a => a.SortOrder)
            .ToListAsync(cancellationToken);
        var payments = paymentRows
            .GroupBy(a => a.OrderId)
            .ToDictionary(g => g.Key, g => g.First());

        var items = pageRows.Select(x =>
        {
            var hasAdvance = payments.ContainsKey(x.Id);
            var advance = hasAdvance ? payments[x.Id] : null;
            var status = hasAdvance
                ? (string.IsNullOrWhiteSpace(advance!.MolliePaymentStatus) ? "open" : advance.MolliePaymentStatus!)
                : "invoice";

            return new OrderSummaryDto
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                CreatedAt = x.CreatedAt,
                CustomerName = string.IsNullOrWhiteSpace(x.CustomerName) ? "—" : x.CustomerName,
                DeliveryTypeId = x.DeliveryTypeId,
                IsAccepted = x.IsAccepted,
                PaymentStatus = status,
                MolliePaymentId = advance?.MolliePaymentId,
                GeneralDiscount = x.GeneralDiscount,
                IsUrgent = x.IsUrgent
            };
        }).ToList();

        return new PagedResult<OrderSummaryDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<OrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await (
            from o in _db.Orders.AsNoTracking()
            join p in _db.Projects.AsNoTracking() on o.ProjectId equals p.ProjectId into projectJoin
            from p in projectJoin.DefaultIfEmpty()
            join c in _db.Customers.AsNoTracking() on p.CustomerId equals c.CustomerId into customerJoin
            from c in customerJoin.DefaultIfEmpty()
            where o.Id == id
            select new OrderEditDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                ProjectId = o.ProjectId,
                CustomerName = c != null ? c.CustomerName : "",
                CreatedAt = o.CreatedAt,
                DeliveryTypeId = o.DeliveryTypeId,
                IsAccepted = o.IsAccepted,
                GeneralDiscount = o.GeneralDiscount,
                CustomerNotes = o.CustomerNotes,
                IsUrgent = o.IsUrgent
            }).FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return null;
        }

        order.AdvancePayments = await _db.OrderAdvancePayments.AsNoTracking()
            .Where(a => a.OrderId == id)
            .OrderBy(a => a.SortOrder)
            .Select(a => new OrderAdvancePaymentDto
            {
                Id = a.Id,
                Name = a.Name,
                Percent = a.Percent,
                Amount = a.Amount,
                SortOrder = a.SortOrder,
                MolliePaymentId = a.MolliePaymentId,
                MolliePaymentStatus = a.MolliePaymentStatus,
                MolliePaidAt = a.MolliePaidAt,
                MollieCheckoutUrl = a.MollieCheckoutUrl
            })
            .ToListAsync(cancellationToken);

        return order;
    }

    public async Task<int> SaveAsync(OrderEditDto dto, CancellationToken cancellationToken = default)
    {
        var currentUser = await _currentUser.GetCurrentUserAsync(cancellationToken);
        Order entity;
        if (dto.Id == 0)
        {
            entity = (Order)AdminCrudDefaults.Create("orders");
            entity.CreatedByUserId = currentUser.ResolveLegacyUserId();
            _db.Orders.Add(entity);
        }
        else
        {
            entity = await _db.Orders.FirstAsync(o => o.Id == dto.Id, cancellationToken);
        }

        if (dto.Id == 0)
        {
            entity.ProjectId = dto.ProjectId;
        }

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
