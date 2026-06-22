using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Domain.Catalog.Products;

namespace WebShopABMATIC.Application.UseCases.Admin;

public sealed class ProductAdminUseCase : IProductAdminPort
{
    private readonly IProductRepository _repository;
    private readonly IProductMediaPort _media;
    private readonly ICurrentUserContext _currentUser;

    public ProductAdminUseCase(IProductRepository repository, IProductMediaPort media, ICurrentUserContext currentUser)
    {
        _repository = repository;
        _media = media;
        _currentUser = currentUser;
    }

    public Task<PagedResult<ProductDto>> GetProductsAsync(ProductListFilter filter, CancellationToken cancellationToken = default) =>
        _repository.GetProductsAsync(filter, cancellationToken);

    public async Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default)
    {
        var dto = await _repository.GetForEditAsync(productId, cancellationToken);
        if (dto is null)
        {
            return null;
        }

        dto.PrimaryImageUrl = await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: false, cancellationToken);
        return dto;
    }

    public async Task<int> SaveAsync(ProductEditDto dto, ProductImageUpload? primaryImage, CancellationToken cancellationToken = default)
    {
        var product = dto.ProductId == 0
            ? Product.Create(dto.NameEn, dto.OrderPartNumber, dto.SupplierId, dto.ManufacturerId, dto.ShowOnWebshop, dto.WebshopDescriptionNl, dto.EanCode)
            : await _repository.GetByIdAsync(dto.ProductId, cancellationToken)
              ?? throw new InvalidOperationException($"Product {dto.ProductId} was not found.");

        product.Update(dto.NameEn, dto.OrderPartNumber, dto.SupplierId, dto.ManufacturerId, dto.ShowOnWebshop, dto.WebshopDescriptionNl, dto.EanCode);

        var productId = await _repository.SaveAsync(product, cancellationToken);

        var currentUser = await _currentUser.GetCurrentUserAsync(cancellationToken);
        var legacyUserId = currentUser.ResolveLegacyUserId();

        if (primaryImage is not null)
        {
            await _media.SavePrimaryImageAsync(productId, primaryImage, publishToWeb: dto.ShowOnWebshop, createdByUserId: legacyUserId, cancellationToken);
        }
        else
        {
            await _media.SetPrimaryImagePublishToWebAsync(productId, dto.ShowOnWebshop, cancellationToken);
        }

        return productId;
    }

    public async Task<bool> DeleteAsync(int productId, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.SoftDeleteAsync(productId, cancellationToken);
        if (deleted)
        {
            await _media.SetPrimaryImagePublishToWebAsync(productId, publishToWeb: false, cancellationToken);
        }

        return deleted;
    }
}
