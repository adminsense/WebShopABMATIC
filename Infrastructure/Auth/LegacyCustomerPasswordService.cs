using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class LegacyCustomerPasswordService(WebShopABMATICDbContext db)
{
    public async Task<(bool Succeeded, string? Error)> ChangePasswordAsync(
        int customerId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        if (customer is null)
        {
            return (false, "Account not found.");
        }

        if (!LegacyWebshopPasswordVerifier.Verify(currentPassword, customer.WebshopPasswordHash, customer.WebshopPasswordSalt))
        {
            return (false, "Current password is incorrect.");
        }

        var (hash, salt) = LegacyWebshopPasswordVerifier.CreateHash(newPassword);
        customer.WebshopPasswordHash = hash;
        customer.WebshopPasswordSalt = salt;
        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }
}
