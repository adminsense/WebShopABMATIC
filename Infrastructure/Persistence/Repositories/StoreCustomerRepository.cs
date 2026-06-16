using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class StoreCustomerRepository : IStoreCustomerRepository
{
    private readonly WebShopABMATICDbContext _db;

    public StoreCustomerRepository(WebShopABMATICDbContext db) => _db = db;

    public async Task<StoreCustomerContext?> GetForStoreUserAsync(StoreUserLookup lookup, CancellationToken cancellationToken = default)
    {
        var customerRow = await ResolveCustomerAsync(lookup, cancellationToken);
        if (customerRow is null)
        {
            return null;
        }

        var projectId = await _db.Projects.AsNoTracking()
            .Where(p => p.CustomerId == customerRow.CustomerId)
            .OrderBy(p => p.ProjectId)
            .Select(p => p.ProjectId)
            .FirstOrDefaultAsync(cancellationToken);

        if (projectId == 0)
        {
            return null;
        }

        return new StoreCustomerContext
        {
            CustomerId = customerRow.CustomerId,
            CustomerTypeId = customerRow.CustomerTypeId,
            DeliveryTypeId = customerRow.DeliveryTypeId,
            BetaaltermijnId = customerRow.BetaaltermijnId,
            ProjectId = projectId,
            AccountManagerUserId = customerRow.AccountManagerUserId
        };
    }

    public async Task<StoreCustomerProfile?> GetProfileAsync(StoreUserLookup lookup, CancellationToken cancellationToken = default)
    {
        return await ResolveCustomerProfileAsync(lookup, cancellationToken);
    }

    public async Task LinkIdentityUserToCustomerAsync(string identityUserId, int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        if (customer is null)
        {
            return;
        }

        customer.IdentityUserId = identityUserId;
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<CustomerRow?> ResolveCustomerAsync(StoreUserLookup lookup, CancellationToken cancellationToken)
    {
        if (lookup.CustomerId is int customerId and > 0)
        {
            var byId = await _db.Customers.AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerRow(c.CustomerId, c.CustomerTypeId, c.DeliveryTypeId, c.BetaaltermijnId, c.AccountManagerUserId))
                .FirstOrDefaultAsync(cancellationToken);

            if (byId is not null)
            {
                return byId;
            }
        }

        if (!string.IsNullOrWhiteSpace(lookup.IdentityUserId))
        {
            var byIdentity = await _db.Customers.AsNoTracking()
                .Where(c => c.IdentityUserId == lookup.IdentityUserId)
                .Select(c => new CustomerRow(c.CustomerId, c.CustomerTypeId, c.DeliveryTypeId, c.BetaaltermijnId, c.AccountManagerUserId))
                .FirstOrDefaultAsync(cancellationToken);

            if (byIdentity is not null)
            {
                return byIdentity;
            }
        }

        if (string.IsNullOrWhiteSpace(lookup.Email))
        {
            return null;
        }

        var normalized = lookup.Email.Trim().ToLowerInvariant();

        return await _db.Customers.AsNoTracking()
            .Where(c =>
                (c.WebshopLogin != null && c.WebshopLogin.ToLower() == normalized) ||
                c.CustomerEmail.ToLower() == normalized)
            .Select(c => new CustomerRow(c.CustomerId, c.CustomerTypeId, c.DeliveryTypeId, c.BetaaltermijnId, c.AccountManagerUserId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<StoreCustomerProfile?> ResolveCustomerProfileAsync(StoreUserLookup lookup, CancellationToken cancellationToken)
    {
        if (lookup.CustomerId is int customerId and > 0)
        {
            var byId = await _db.Customers.AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new StoreCustomerProfile
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerEmail = c.CustomerEmail,
                    CustomerPhone = c.CustomerPhone
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (byId is not null)
            {
                return byId;
            }
        }

        if (!string.IsNullOrWhiteSpace(lookup.IdentityUserId))
        {
            var byIdentity = await _db.Customers.AsNoTracking()
                .Where(c => c.IdentityUserId == lookup.IdentityUserId)
                .Select(c => new StoreCustomerProfile
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    CustomerEmail = c.CustomerEmail,
                    CustomerPhone = c.CustomerPhone
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (byIdentity is not null)
            {
                return byIdentity;
            }
        }

        if (string.IsNullOrWhiteSpace(lookup.Email))
        {
            return null;
        }

        var normalized = lookup.Email.Trim().ToLowerInvariant();

        return await _db.Customers.AsNoTracking()
            .Where(c =>
                (c.WebshopLogin != null && c.WebshopLogin.ToLower() == normalized) ||
                c.CustomerEmail.ToLower() == normalized)
            .Select(c => new StoreCustomerProfile
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerEmail = c.CustomerEmail,
                CustomerPhone = c.CustomerPhone
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    private sealed record CustomerRow(int CustomerId, int CustomerTypeId, int DeliveryTypeId, int BetaaltermijnId, int AccountManagerUserId);
}
