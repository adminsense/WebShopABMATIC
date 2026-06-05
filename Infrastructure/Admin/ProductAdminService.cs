using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Admin;

public sealed class ProductAdminService : IProductAdminPort
{
    private readonly WebShopABMATICDbContext _db;
    private readonly IProductMediaPort _media;

    public ProductAdminService(WebShopABMATICDbContext db, IProductMediaPort media)
    {
        _db = db;
        _media = media;
    }

    public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Products.AsNoTracking().Where(p => !p.IsInactive);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(p =>
                p.NameEn.Contains(term) ||
                (p.OrderPartNumber != null && p.OrderPartNumber.Contains(term)) ||
                (p.EanCode != null && p.EanCode.Contains(term)));
        }

        if (filter.ShowOnWebshop is true)
        {
            query = query.Where(p => p.ShowOnWebshop == true);
        }

        if (filter.ModifiedOnly)
        {
            query = query.Where(p => p.LastModifiedAt != null);
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(p => p.NameEn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                NameEn = p.NameEn ?? string.Empty,
                OrderPartNumber = p.OrderPartNumber,
                SupplierId = p.SupplierId,
                ManufacturerId = p.ManufacturerId,
                ShowOnWebshop = p.ShowOnWebshop == true,
                WebshopDescriptionNl = p.WebshopDescriptionNl,
                EanCode = p.EanCode,
                IsInactive = p.IsInactive
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default)
    {
        var dto = await _db.Products.AsNoTracking()
            .Where(p => p.ProductId == productId)
            .Select(p => new ProductEditDto
            {
                ProductId = p.ProductId,
                NameEn = p.NameEn,
                OrderPartNumber = p.OrderPartNumber,
                SupplierId = p.SupplierId,
                ManufacturerId = p.ManufacturerId,
                ShowOnWebshop = p.ShowOnWebshop == true,
                WebshopDescriptionNl = p.WebshopDescriptionNl,
                EanCode = p.EanCode
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
        {
            return null;
        }

        dto.PrimaryImageUrl = await _media.GetPrimaryImageUrlAsync(productId, webPublishedOnly: false, cancellationToken);
        return dto;
    }

    public async Task<int> SaveAsync(ProductEditDto dto, ProductImageUpload? primaryImage, CancellationToken cancellationToken = default)
    {
        Product entity;
        if (dto.ProductId == 0)
        {
            entity = ProductEntityFactory.CreateNew(dto);
            _db.Products.Add(entity);
        }
        else
        {
            entity = await _db.Products.FirstAsync(p => p.ProductId == dto.ProductId, cancellationToken);
        }

        entity.NameEn = dto.NameEn;
        entity.NameNl = dto.NameEn;
        entity.NameFr = dto.NameEn;
        entity.DescriptionEn = dto.WebshopDescriptionNl;
        entity.DescriptionNl = dto.WebshopDescriptionNl;
        entity.DescriptionFr = dto.WebshopDescriptionNl;
        entity.OrderPartNumber = dto.OrderPartNumber;
        entity.StockNumber = dto.OrderPartNumber;
        entity.SupplierId = dto.SupplierId;
        entity.ManufacturerId = dto.ManufacturerId;
        entity.ShowOnWebshop = dto.ShowOnWebshop;
        entity.WebshopDescriptionNl = dto.WebshopDescriptionNl;
        entity.EanCode = dto.EanCode;
        entity.LastModifiedAt = DateTime.UtcNow;
        entity.LastModifiedBy = "admin";

        await _db.SaveChangesAsync(cancellationToken);

        if (primaryImage is not null)
        {
            await _media.SavePrimaryImageAsync(
                entity.ProductId,
                primaryImage,
                publishToWeb: dto.ShowOnWebshop,
                createdByUserId: 1,
                cancellationToken);
        }
        else
        {
            await _media.SetPrimaryImagePublishToWebAsync(entity.ProductId, dto.ShowOnWebshop, cancellationToken);
        }

        return entity.ProductId;
    }

    public async Task<bool> DeleteAsync(int productId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        entity.IsInactive = true;
        entity.ShowOnWebshop = false;
        entity.LastModifiedAt = DateTime.UtcNow;
        entity.LastModifiedBy = "admin";
        await _db.SaveChangesAsync(cancellationToken);
        await _media.SetPrimaryImagePublishToWebAsync(productId, publishToWeb: false, cancellationToken);
        return true;
    }
}
