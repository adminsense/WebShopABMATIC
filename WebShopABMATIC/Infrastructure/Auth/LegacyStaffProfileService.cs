using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Data.Persistence;

namespace WebShopABMATIC.Infrastructure.Auth;

public sealed class LegacyStaffProfileService(WebShopABMATICDbContext db) : ILegacyStaffProfilePort
{
    public async Task<LegacyStaffProfileDto?> GetAsync(int staffUserId, CancellationToken cancellationToken = default)
    {
        var staff = await db.StaffUsers.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == staffUserId, cancellationToken);

        return staff is null
            ? null
            : new LegacyStaffProfileDto
            {
                StaffUserId = staff.Id,
                Login = staff.Login,
                Email = staff.EmailTemplate,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Phone = staff.Tel
            };
    }

    public async Task<(bool Succeeded, string? Error)> SaveAsync(
        int staffUserId,
        LegacyStaffProfileUpdate update,
        CancellationToken cancellationToken = default)
    {
        var staff = await db.StaffUsers.FirstOrDefaultAsync(u => u.Id == staffUserId, cancellationToken);
        if (staff is null)
        {
            return (false, "Staff user not found.");
        }

        staff.FirstName = update.FirstName.Trim();
        staff.LastName = update.LastName.Trim();
        staff.Tel = update.Phone?.Trim();
        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }

    public async Task<(bool Succeeded, string? Error)> ChangePasswordAsync(
        int staffUserId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var staff = await db.StaffUsers.FirstOrDefaultAsync(u => u.Id == staffUserId, cancellationToken);
        if (staff is null)
        {
            return (false, "Staff user not found.");
        }

        if (!string.Equals(staff.Password, currentPassword, StringComparison.Ordinal))
        {
            return (false, "Current password is incorrect.");
        }

        staff.Password = newPassword;
        await db.SaveChangesAsync(cancellationToken);
        return (true, null);
    }
}
