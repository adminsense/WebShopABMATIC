using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockMovementDto
{
    public int Id { get; init; }
    public DateTime Timestamp { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = "";
    public string PartNumber { get; init; } = "";
    public string LocationName { get; init; } = "";
    public decimal Quantity { get; init; }
    public bool IsReservation { get; init; }
    public int? OrderLineId { get; init; }
    public string? Notes { get; init; }
}

public sealed class StockMovementListFilter
{
    public string? Search { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? StockLocationId { get; set; }
    public bool? ReservationsOnly { get; set; }
    public bool? HasOrderLine { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
