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

    public ProductAdminService(WebShopABMATICDbContext db) => _db = db;

    public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductListFilter filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _db.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim();
                query = query.Where(p =>
                    p.NameEn.Contains(term) ||
                    p.OrderPartNumber.Contains(term) ||
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
                    NameEn = p.NameEn,
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
        catch
        {
            return new PagedResult<ProductDto>
            {
                Items = [],
                TotalCount = 0,
                Page = 1,
                PageSize = filter.PageSize
            };
        }
    }

    public async Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _db.Products.AsNoTracking()
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
    }

    public async Task<int> SaveAsync(ProductEditDto dto, CancellationToken cancellationToken = default)
    {
        Product entity;
        if (dto.ProductId == 0)
        {
            entity = new Product
            {
                NameEn = dto.NameEn,
                NameNl = dto.NameEn,
                NameFr = dto.NameEn,
                DescriptionEn = dto.WebshopDescriptionNl,
                DescriptionNl = dto.WebshopDescriptionNl,
                DescriptionFr = dto.WebshopDescriptionNl,
                ShortNotesEn = string.Empty,
                ShortNotesNl = string.Empty,
                ShortNotesFr = string.Empty,
                OrderPartNumber = dto.OrderPartNumber,
                StockNumber = dto.OrderPartNumber,
                WebshopDescriptionNl = dto.WebshopDescriptionNl,
                SupplierId = dto.SupplierId,
                ManufacturerId = dto.ManufacturerId,
                ShowOnWebshop = dto.ShowOnWebshop
            };
            _db.Products.Add(entity);
        }
        else
        {
            entity = await _db.Products.FirstAsync(p => p.ProductId == dto.ProductId, cancellationToken);
        }

        entity.NameEn = dto.NameEn;
        entity.NameNl = dto.NameEn;
        entity.NameFr = dto.NameEn;
        entity.OrderPartNumber = dto.OrderPartNumber;
        entity.SupplierId = dto.SupplierId;
        entity.ManufacturerId = dto.ManufacturerId;
        entity.ShowOnWebshop = dto.ShowOnWebshop;
        entity.WebshopDescriptionNl = dto.WebshopDescriptionNl;
        entity.EanCode = dto.EanCode;
        entity.LastModifiedAt = DateTime.UtcNow;
        entity.LastModifiedBy = "admin";

        await _db.SaveChangesAsync(cancellationToken);
        return entity.ProductId;
    }
}
