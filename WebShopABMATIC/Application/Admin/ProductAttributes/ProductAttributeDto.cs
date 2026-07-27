using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.ProductAttributes;

public sealed class ProductAttributeDto
{
    public int Id { get; init; }
    public string NameEn { get; init; } = string.Empty;
    public string NameNl { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}

public sealed class ProductAttributeEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameNl { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public sealed class ProductAttributeListFilter
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = AdminGridDefaults.PageSize;
}

public sealed class ProductAttributeAssignmentProductDto
{
    public int ProductId { get; init; }
    public string NameNl { get; init; } = string.Empty;
    public string NameEn { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public string? OrderPartNumber { get; init; }
    public string? EanCode { get; init; }
}

public sealed class ProductAttributeValueDto
{
    public int Id { get; init; }
    public int ProductAttributeId { get; init; }
    public string AttributeNameEn { get; init; } = string.Empty;
    public string AttributeNameNl { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public sealed class ProductAttributeValueEditDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ProductAttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
}

public sealed class ProductAttributeAssignmentDto
{
    public ProductAttributeAssignmentProductDto Product { get; init; } = new();
    public IReadOnlyList<ProductAttributeValueDto> Values { get; init; } = [];
    public IReadOnlyList<ProductAttributeDto> AvailableAttributes { get; init; } = [];
}
