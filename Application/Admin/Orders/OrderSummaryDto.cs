namespace WebShopABMATIC.Application.Admin.Orders;

public sealed class OrderSummaryDto
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public int ProjectId { get; init; }
    public int DeliveryTypeId { get; init; }
    public bool IsAccepted { get; init; }
}

public sealed class OrderListFilter
{
    public string? Search { get; set; }
    public bool? IsAccepted { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
