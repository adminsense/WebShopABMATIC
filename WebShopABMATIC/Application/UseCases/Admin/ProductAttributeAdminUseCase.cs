using WebShopABMATIC.Application.Admin.ProductAttributes;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ProductAttributeAdminUseCase : IProductAttributeAdminPort
{
    private readonly IProductAttributeRepository _repository;

    public ProductAttributeAdminUseCase(IProductAttributeRepository repository) => _repository = repository;

    public Task<PagedResult<ProductAttributeDto>> GetAttributesAsync(ProductAttributeListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetAttributesAsync(filter, cancellationToken);

    public Task<ProductAttributeEditDto?> GetForEditAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.GetForEditAsync(id, cancellationToken);

    public Task<int> SaveAsync(ProductAttributeEditDto dto, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(dto, cancellationToken);

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _repository.HasValuesAsync(id, cancellationToken))
        {
            throw new InvalidOperationException("Cannot delete an attribute that is assigned to products.");
        }

        return await _repository.DeleteAsync(id, cancellationToken);
    }
}

public sealed class ProductAttributeAssignmentAdminUseCase : IProductAttributeAssignmentAdminPort
{
    private readonly IProductAttributeAssignmentRepository _repository;

    public ProductAttributeAssignmentAdminUseCase(IProductAttributeAssignmentRepository repository) =>
        _repository = repository;

    public Task<PagedResult<ProductAttributeAssignmentProductDto>> SearchProductsAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default) =>
        _repository.SearchProductsAsync(search, page, pageSize, cancellationToken);

    public Task<ProductAttributeAssignmentDto?> GetAssignmentAsync(int productId, CancellationToken cancellationToken = default) =>
        _repository.GetAssignmentAsync(productId, cancellationToken);

    public Task<int> SaveValueAsync(ProductAttributeValueEditDto dto, CancellationToken cancellationToken = default) =>
        _repository.SaveValueAsync(dto, cancellationToken);

    public Task<bool> DeleteValueAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.DeleteValueAsync(id, cancellationToken);
}
