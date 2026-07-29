using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Admin.ProductAttributes;

public sealed class ProductAttributeDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? DataType { get; init; }
    public string? Unit { get; init; }
}

public sealed class ProductAttributeEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DataType { get; set; }
    public string? Unit { get; set; }
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
    public string AttributeName { get; init; } = string.Empty;
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
