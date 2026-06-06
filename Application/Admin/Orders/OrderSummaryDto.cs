namespace WebShopABMATIC.Application.Admin.Orders;

public sealed class OrderSummaryDto
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public int ProjectId { get; init; }
    public int DeliveryTypeId { get; init; }
    public bool IsAccepted { get; init; }
    public decimal GeneralDiscount { get; init; }
    public bool IsUrgent { get; init; }
}

public sealed class OrderEditDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int DeliveryTypeId { get; set; }
    public bool IsAccepted { get; set; }
    public decimal GeneralDiscount { get; set; }
    public string? CustomerNotes { get; set; }
    public bool IsUrgent { get; set; }
}

public sealed class OrderListFilter
{
    public string? Search { get; set; }
    public bool? IsAccepted { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
