namespace WebShopABMATIC.Application.Admin.PaymentMethods;

public sealed class PaymentMethodDto
{
    public int Id { get; init; }
    public string NameEn { get; init; } = string.Empty;
    public string NameNl { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public bool IsPrePay { get; init; }
    public bool IsPostPay { get; init; }
}

public sealed class PaymentMethodEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameNl { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public bool IsPrePay { get; set; }
    public bool IsPostPay { get; set; }
}

public sealed class PaymentMethodListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
