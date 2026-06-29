using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Infrastructure.Persistence;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly WebShopABMATICDbContext _db;
    private readonly ICurrentUserContext _currentUser;

    public CustomerRepository(
        WebShopABMATICDbContext db,
        ICurrentUserContext currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _db.Customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(c =>
                c.CustomerName.Contains(term) ||
                c.CustomerEmail.Contains(term) ||
                (c.WebshopLogin != null && c.WebshopLogin.Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .OrderBy(c => c.CustomerName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                WebshopLogin = c.WebshopLogin,
                CustomerTypeId = c.CustomerTypeId,
                CustomerEmail = c.CustomerEmail,
                CustomerPhone = c.CustomerPhone,
                CustomerCityId = c.CustomerCityId,
                Locked = c.Locked,
                HasWebshopAccount = c.WebshopPasswordHash != null && c.WebshopPasswordHash != ""
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<CustomerEditDto?> GetForEditAsync(int customerId, CancellationToken cancellationToken = default) =>
        await _db.Customers.AsNoTracking()
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerEditDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                WebshopLogin = c.WebshopLogin,
                CustomerEmail = c.CustomerEmail,
                CustomerPhone = c.CustomerPhone,
                CustomerTypeId = c.CustomerTypeId,
                CustomerCityId = c.CustomerCityId,
                CustomerStreet = c.CustomerStreet,
                CustomerHouseNumber = c.CustomerHouseNumber,
                CustomerBox = c.CustomerBox,
                CustomerVatNumber = c.CustomerVatNumber,
                DeliveryTypeId = c.DeliveryTypeId,
                BetaaltermijnId = c.BetaaltermijnId,
                Locked = c.Locked,
                HasWebshopAccount = c.WebshopPasswordHash != null && c.WebshopPasswordHash != ""
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<int> SaveAsync(CustomerEditDto dto, CancellationToken cancellationToken = default)
    {
        var currentUser = await _currentUser.GetCurrentUserAsync(cancellationToken);
        var now = DateTime.UtcNow;

        Customer entity;
        if (dto.CustomerId == 0)
        {
            entity = (Customer)AdminCrudDefaults.Create("customers");
            entity.CreatedAt = now;
            entity.CreatedBy = currentUser.AuditLabel;
            _db.Customers.Add(entity);
        }
        else
        {
            entity = await _db.Customers.FirstAsync(c => c.CustomerId == dto.CustomerId, cancellationToken);
        }

        entity.CustomerName = dto.CustomerName;
        entity.WebshopLogin = dto.WebshopLogin;
        entity.CustomerEmail = dto.CustomerEmail;
        entity.CustomerPhone = dto.CustomerPhone;
        entity.CustomerTypeId = dto.CustomerTypeId;
        entity.CustomerCityId = dto.CustomerCityId;
        entity.CustomerStreet = dto.CustomerStreet;
        entity.CustomerHouseNumber = dto.CustomerHouseNumber;
        entity.CustomerBox = dto.CustomerBox;
        entity.CustomerVatNumber = dto.CustomerVatNumber;
        entity.DeliveryTypeId = dto.DeliveryTypeId;
        entity.BetaaltermijnId = dto.BetaaltermijnId;
        entity.Locked = dto.Locked;
        entity.ModifiedAt = now;
        entity.ModifiedBy = currentUser.AuditLabel;

        await _db.SaveChangesAsync(cancellationToken);
        return entity.CustomerId;
    }

    public async Task<bool> DeleteAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        if (entity is null) return false;
        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PasswordResetResult> ResetWebshopPasswordAsync(int customerId, string? newPassword = null, CancellationToken cancellationToken = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        if (customer is null || string.IsNullOrWhiteSpace(customer.WebshopLogin))
        {
            return new PasswordResetResult
            {
                Succeeded = false,
                Errors = ["Customer has no linked webshop account."]
            };
        }

        var password = string.IsNullOrWhiteSpace(newPassword)
            ? LegacyPasswordGenerator.GenerateTemporaryPassword()
            : newPassword;

        var (hash, salt) = LegacyWebshopPasswordVerifier.CreateHash(password);
        customer.WebshopPasswordHash = hash;
        customer.WebshopPasswordSalt = salt;
        await _db.SaveChangesAsync(cancellationToken);

        return new PasswordResetResult
        {
            Succeeded = true,
            TemporaryPassword = string.IsNullOrWhiteSpace(newPassword) ? password : null
        };
    }
}
