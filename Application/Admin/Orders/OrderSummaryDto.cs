using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.Orders;

public sealed class OrderSummaryDto
{
    public int Id { get; init; }
    public int? OrderNumber { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CustomerName { get; init; } = "";
    public int DeliveryTypeId { get; init; }
    public bool IsAccepted { get; init; }
    public string PaymentStatus { get; init; } = "";
    public string? MolliePaymentId { get; init; }
    public decimal GeneralDiscount { get; init; }
    public bool IsUrgent { get; init; }
}

public sealed class OrderAdvancePaymentDto
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public decimal Percent { get; init; }
    public decimal? Amount { get; init; }
    public int SortOrder { get; init; }
    public string? MolliePaymentId { get; init; }
    public string? MolliePaymentStatus { get; init; }
    public DateTime? MolliePaidAt { get; init; }
    public string? MollieCheckoutUrl { get; init; }
}

public sealed class OrderEditDto
{
    public int Id { get; set; }
    public int? OrderNumber { get; set; }
    public int ProjectId { get; set; }
    public string CustomerName { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public int DeliveryTypeId { get; set; }
    public bool IsAccepted { get; set; }
    public decimal GeneralDiscount { get; set; }
    public string? CustomerNotes { get; set; }
    public bool IsUrgent { get; set; }
    public IReadOnlyList<OrderAdvancePaymentDto> AdvancePayments { get; set; } = [];
}

public sealed class OrderListFilter
{
    public string? Search { get; set; }
    public bool? IsAccepted { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}
