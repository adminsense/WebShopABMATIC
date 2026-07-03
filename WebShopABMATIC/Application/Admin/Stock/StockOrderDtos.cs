using System.ComponentModel.DataAnnotations;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.Stock;

public sealed class StockOrderSummaryDto
{
    public int Id { get; init; }
    public int SupplierId { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpectedDeliveryDate { get; init; }
    public bool IsCompleted { get; init; }
    public decimal? TotalAmount { get; init; }
    public int LineCount { get; init; }
    public int LinesFullyDelivered { get; init; }
}

public sealed class StockOrderLineEditDto
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    [Required]
    [MaxLength(500)]
    public string ProductName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal QuantityOrdered { get; set; } = 1;

    public decimal QuantityDelivered { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PurchaseUnitPrice { get; set; }

    public decimal PurchaseTotalPrice { get; set; }

    public bool? Besteld { get; set; }

    public bool? Geleverd { get; set; }
}

public sealed class StockOrderEditDto
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int SupplierId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [MaxLength(2000)]
    public string? InternalNotes { get; set; }

    public bool IsCompleted { get; set; }

    public List<StockOrderLineEditDto> Lines { get; set; } = [];
}

public sealed class StockOrderLookupsDto
{
    public IReadOnlyList<StockLookupItemDto> Suppliers { get; init; } = [];
}

public sealed class StockOrderListFilter
{
    public string? Search { get; set; }
    public bool? OpenOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
