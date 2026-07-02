using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StockOrderRepository : IStockOrderRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StockOrderRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<StockOrderSummaryDto>> GetOrdersAsync(
        StockOrderListFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query =
            from order in _db.StockOrders.AsNoTracking()
            join supplier in _db.Suppliers.AsNoTracking() on order.SupplierId equals supplier.SupplierId into supplierJoin
            from supplier in supplierJoin.DefaultIfEmpty()
            select new { order, SupplierName = supplier != null ? supplier.Name : $"Supplier {order.SupplierId}" };

        if (filter.OpenOnly == true)
        {
            query = query.Where(x => !x.order.IsCompleted);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            if (int.TryParse(term, out var orderId))
            {
                query = query.Where(x => x.order.Id == orderId || x.order.SupplierId == orderId);
            }
            else
            {
                query = query.Where(x => x.SupplierName.Contains(term));
            }
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var pageRows = await query
            .OrderByDescending(x => x.order.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new { x.order.Id, x.order.SupplierId, x.SupplierName, x.order.CreatedAt, x.order.ExpectedDeliveryDate, x.order.IsCompleted, x.order.TotalAmount })
            .ToListAsync(cancellationToken);

        var orderIds = pageRows.Select(x => x.Id).ToList();
        var lineStats = await _db.StockOrderLines.AsNoTracking()
            .Where(l => orderIds.Contains(l.StockOrderId))
            .GroupBy(l => l.StockOrderId)
            .Select(g => new
            {
                StockOrderId = g.Key,
                LineCount = g.Count(),
                LinesFullyDelivered = g.Count(l => l.Geleverd == true || l.QuantityDelivered >= l.QuantityOrdered)
            })
            .ToListAsync(cancellationToken);

        var statsByOrder = lineStats.ToDictionary(x => x.StockOrderId);

        var items = pageRows.Select(x =>
        {
            statsByOrder.TryGetValue(x.Id, out var stats);
            return new StockOrderSummaryDto
            {
                Id = x.Id,
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierName,
                CreatedAt = x.CreatedAt,
                ExpectedDeliveryDate = x.ExpectedDeliveryDate,
                IsCompleted = x.IsCompleted,
                TotalAmount = x.TotalAmount,
                LineCount = stats?.LineCount ?? 0,
                LinesFullyDelivered = stats?.LinesFullyDelivered ?? 0
            };
        }).ToList();

        return new PagedResult<StockOrderSummaryDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<StockOrderEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var header = await _db.StockOrders.AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new StockOrderEditDto
            {
                Id = o.Id,
                SupplierId = o.SupplierId,
                OrderDate = o.OrderDate,
                ExpectedDeliveryDate = o.ExpectedDeliveryDate,
                Notes = o.Notes,
                InternalNotes = o.InternalNotes,
                IsCompleted = o.IsCompleted
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (header is null)
        {
            return null;
        }

        header.Lines = await _db.StockOrderLines.AsNoTracking()
            .Where(l => l.StockOrderId == id)
            .OrderBy(l => l.Id)
            .Select(l => new StockOrderLineEditDto
            {
                Id = l.Id,
                ProductId = l.ProductId,
                ProductName = l.ProductName,
                QuantityOrdered = l.QuantityOrdered,
                QuantityDelivered = l.QuantityDelivered,
                PurchaseUnitPrice = l.PurchaseUnitPrice,
                PurchaseTotalPrice = l.PurchaseTotalPrice,
                Besteld = l.Besteld,
                Geleverd = l.Geleverd
            })
            .ToListAsync(cancellationToken);

        return header;
    }

    public async Task<StockOrderLookupsDto> GetLookupsAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = await _db.Suppliers.AsNoTracking()
            .Where(s => s.IsInactive != true)
            .OrderBy(s => s.Name)
            .Select(s => new StockLookupItemDto { Id = s.SupplierId, Name = s.Name })
            .ToListAsync(cancellationToken);

        return new StockOrderLookupsDto { Suppliers = suppliers };
    }

    public async Task<int> SaveAsync(StockOrderEditDto dto, int userId, CancellationToken cancellationToken = default)
    {
        if (dto.Lines.Count == 0)
        {
            throw new InvalidOperationException("At least one order line is required.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);

        StockOrder header;
        if (dto.Id > 0)
        {
            header = await _db.StockOrders.FirstOrDefaultAsync(o => o.Id == dto.Id, cancellationToken)
                ?? throw new InvalidOperationException($"Purchase order {dto.Id} not found.");

            header.SupplierId = dto.SupplierId;
            header.OrderDate = dto.OrderDate;
            header.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
            header.Notes = dto.Notes;
            header.InternalNotes = dto.InternalNotes;
            header.IsCompleted = dto.IsCompleted;
        }
        else
        {
            header = new StockOrder
            {
                SupplierId = dto.SupplierId,
                CreatedAt = DateTime.UtcNow,
                OrderDate = dto.OrderDate ?? DateTime.UtcNow.Date,
                ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
                Notes = dto.Notes,
                InternalNotes = dto.InternalNotes,
                IsCompleted = false,
                UserId = userId
            };
            _db.StockOrders.Add(header);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var existingLines = await _db.StockOrderLines
            .Where(l => l.StockOrderId == header.Id)
            .ToListAsync(cancellationToken);

        var incomingIds = dto.Lines.Where(l => l.Id > 0).Select(l => l.Id).ToHashSet();
        foreach (var removed in existingLines.Where(l => !incomingIds.Contains(l.Id)))
        {
            var hasDelivery = await _db.StockOrderDeliveries
                .AnyAsync(d => d.StockOrderDetail == removed.Id, cancellationToken);
            if (hasDelivery)
            {
                throw new InvalidOperationException(
                    $"Cannot remove line #{removed.Id} because deliveries were already booked.");
            }

            _db.StockOrderLines.Remove(removed);
        }

        var lineIndex = 0;
        foreach (var lineDto in dto.Lines)
        {
            lineIndex++;
            var totalPrice = lineDto.QuantityOrdered * lineDto.PurchaseUnitPrice;
            var orderNumber = $"PO-{header.Id}-{lineIndex}";

            if (lineDto.Id > 0)
            {
                var line = existingLines.FirstOrDefault(l => l.Id == lineDto.Id)
                    ?? throw new InvalidOperationException($"Line #{lineDto.Id} not found on PO {header.Id}.");

                if (line.QuantityDelivered > lineDto.QuantityOrdered)
                {
                    throw new InvalidOperationException(
                        $"Ordered quantity cannot be less than already delivered ({line.QuantityDelivered:N2}).");
                }

                line.ProductId = lineDto.ProductId;
                line.ProductName = lineDto.ProductName.Trim();
                line.QuantityOrdered = lineDto.QuantityOrdered;
                line.PurchaseUnitPrice = lineDto.PurchaseUnitPrice;
                line.PurchaseTotalPrice = totalPrice;
                line.Besteld = lineDto.Besteld ?? true;
                line.Geleverd = line.QuantityDelivered >= lineDto.QuantityOrdered;
                if (line.Geleverd == true && line.DeliveredAt is null)
                {
                    line.DeliveredAt = DateTime.UtcNow;
                }
            }
            else
            {
                var productName = lineDto.ProductName.Trim();
                if (lineDto.ProductId is > 0 && string.IsNullOrWhiteSpace(productName))
                {
                    productName = await _db.Products.AsNoTracking()
                        .Where(p => p.ProductId == lineDto.ProductId)
                        .Select(p => p.NameEn)
                        .FirstOrDefaultAsync(cancellationToken) ?? $"Product {lineDto.ProductId}";
                }

                _db.StockOrderLines.Add(new StockOrderLine
                {
                    StockOrderId = header.Id,
                    ProductId = lineDto.ProductId,
                    ProductName = productName,
                    QuantityOrdered = lineDto.QuantityOrdered,
                    QuantityDelivered = 0,
                    PurchaseUnitPrice = lineDto.PurchaseUnitPrice,
                    PurchaseTotalPrice = totalPrice,
                    OrderNumber = orderNumber,
                    PackSize = "1",
                    Unit = "pcs",
                    LijnOK = true,
                    Besteld = true,
                    OrderedAt = DateTime.UtcNow,
                    Geleverd = false,
                    QuantityProcessedToStock = 0
                });
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        var allLines = await _db.StockOrderLines
            .Where(l => l.StockOrderId == header.Id)
            .ToListAsync(cancellationToken);

        header.TotalAmount = allLines.Sum(l => l.PurchaseTotalPrice);
        header.IsCompleted = dto.IsCompleted || allLines.All(l => l.Geleverd == true || l.QuantityDelivered >= l.QuantityOrdered);

        await _db.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return header.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var header = await _db.StockOrders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (header is null)
        {
            return false;
        }

        var lineIds = await _db.StockOrderLines.AsNoTracking()
            .Where(l => l.StockOrderId == id)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        if (lineIds.Count > 0)
        {
            var hasDeliveries = await _db.StockOrderDeliveries.AsNoTracking()
                .AnyAsync(d => lineIds.Contains(d.StockOrderDetail), cancellationToken);
            if (hasDeliveries)
            {
                throw new InvalidOperationException("Cannot delete a purchase order with booked deliveries.");
            }
        }

        var lines = await _db.StockOrderLines.Where(l => l.StockOrderId == id).ToListAsync(cancellationToken);
        _db.StockOrderLines.RemoveRange(lines);
        _db.StockOrders.Remove(header);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
