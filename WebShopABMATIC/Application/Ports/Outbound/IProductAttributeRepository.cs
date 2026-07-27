using WebShopABMATIC.Application.Admin.ProductAttributes;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IProductAttributeRepository
{
    Task<PagedResult<ProductAttributeDto>> GetAttributesAsync(ProductAttributeListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductAttributeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductAttributeEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> HasValuesAsync(int attributeId, CancellationToken cancellationToken = default);
}

public interface IProductAttributeAssignmentRepository
{
    Task<PagedResult<ProductAttributeAssignmentProductDto>> SearchProductsAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ProductAttributeAssignmentDto?> GetAssignmentAsync(int productId, CancellationToken cancellationToken = default);

    Task<int> SaveValueAsync(ProductAttributeValueEditDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteValueAsync(int id, CancellationToken cancellationToken = default);
}
