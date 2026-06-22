using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StockMovementRepository : IStockMovementRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StockMovementRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<StockMovementDto>> GetMovementsAsync(StockMovementListFilter filter, CancellationToken cancellationToken = default)
    {
        var query =
            from m in _db.StockMovements.AsNoTracking()
            join p in _db.Products.AsNoTracking() on m.ProductId equals p.ProductId
            join psl in _db.ProductStockLocations.AsNoTracking() on m.ProductStockLocatieId equals psl.Id
            join sl in _db.StockLocations.AsNoTracking() on psl.StockLocationId equals sl.Id
            select new { m, p, sl };

        if (filter.FromDate.HasValue)
        {
            var from = filter.FromDate.Value.Date;
            query = query.Where(x => x.m.Timestamp >= from);
        }

        if (filter.ToDate.HasValue)
        {
            var to = filter.ToDate.Value.Date.AddDays(1);
            query = query.Where(x => x.m.Timestamp < to);
        }

        if (filter.StockLocationId.HasValue)
        {
            var locId = filter.StockLocationId.Value;
            query = query.Where(x => x.sl.Id == locId);
        }

        if (filter.ReservationsOnly == true)
        {
            query = query.Where(x => x.m.IsReservation == true);
        }

        if (filter.HasOrderLine == true)
        {
            query = query.Where(x => x.m.OrderLineId != null);
        }
        else if (filter.HasOrderLine == false)
        {
            query = query.Where(x => x.m.OrderLineId == null);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            if (int.TryParse(term, out var productId))
            {
                query = query.Where(x => x.m.ProductId == productId);
            }
            else
            {
                query = query.Where(x =>
                    x.p.NameEn.Contains(term) ||
                    x.p.OrderPartNumber.Contains(term));
            }
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderByDescending(x => x.m.Timestamp)
            .ThenByDescending(x => x.m.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new StockMovementDto
            {
                Id = x.m.Id,
                Timestamp = x.m.Timestamp,
                ProductId = x.m.ProductId,
                ProductName = x.p.NameEn,
                PartNumber = x.p.OrderPartNumber,
                LocationName = x.sl.Name,
                Quantity = x.m.Quantity,
                IsReservation = x.m.IsReservation == true,
                OrderLineId = x.m.OrderLineId,
                Notes = x.m.Notes
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<StockMovementDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }
}
