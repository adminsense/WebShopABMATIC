using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Persistence.Products;
using WebShopABMATIC.Infrastructure.Persistence.Mappers;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Domain.Catalog.Products;
using Microsoft.EntityFrameworkCore;
using PersistenceProduct = WebShopABMATIC.Data.Entities.Product;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly WebShopABMATICDbContext _db;
    private readonly ICurrentUserContext _currentUser;

    public ProductRepository(WebShopABMATICDbContext db, ICurrentUserContext currentUser)
    {
        _db = db;
        _currentUser = currentUser;
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

    public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

        return entity is null ? null : ProductPersistenceMapper.ToDomain(entity);
    }

    public async Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default) =>
        await _db.Products.AsNoTracking()
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

    public async Task<int> SaveAsync(Product product, CancellationToken cancellationToken = default)
    {
        PersistenceProduct entity;
        if (product.ProductId == 0)
        {
            entity = ProductEntityFactory.CreateNew(product);
            _db.Products.Add(entity);
        }
        else
        {
            entity = await _db.Products.FirstAsync(p => p.ProductId == product.ProductId, cancellationToken);
        }

        ProductPersistenceMapper.ApplyToEntity(product, entity, (await _currentUser.GetCurrentUserAsync(cancellationToken)).AuditLabel);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.ProductId;
    }

    public async Task<bool> SoftDeleteAsync(int productId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        var domain = ProductPersistenceMapper.ToDomain(entity);
        domain.Deactivate();
        ProductPersistenceMapper.ApplyToEntity(domain, entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
