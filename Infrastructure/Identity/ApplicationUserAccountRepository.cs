using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.UserAccounts;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Audit;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class ApplicationUserAccountRepository : IApplicationUserAccountRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityPasswordPort _passwordPort;
    private readonly IStoreCustomerRepository _storeCustomers;
    private readonly IAuditService _audit;

    public ApplicationUserAccountRepository(
        UserManager<ApplicationUser> userManager,
        IIdentityPasswordPort passwordPort,
        IStoreCustomerRepository storeCustomers,
        IAuditService audit)
    {
        _userManager = userManager;
        _passwordPort = passwordPort;
        _storeCustomers = storeCustomers;
        _audit = audit;
    }

    public async Task<PagedResult<ApplicationUserAccountDto>> GetAccountsAsync(
        ApplicationUserAccountListFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(term)) ||
                u.FirstName.Contains(term) ||
                u.LastName.Contains(term));
        }

        var candidates = await query
            .OrderBy(u => u.Email)
            .ToListAsync(cancellationToken);

        var items = new List<ApplicationUserAccountDto>();
        foreach (var user in candidates)
        {
            var roles = await _userManager.GetRolesAsync(user);
            items.Add(MapDto(user, roles));
        }

        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var pageItems = items
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<ApplicationUserAccountDto>
        {
            Items = pageItems,
            TotalCount = items.Count,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApplicationUserAccountEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        return MapEditDto(user, roles);
    }

    public async Task<ApplicationUserAccountSaveResult> SaveAsync(
        ApplicationUserAccountEditDto dto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return Fail("Email is required.");
        }

        if (!dto.IsAdmin && !dto.IsManager && !dto.IsCustomer)
        {
            return Fail("Select at least one role (Admin, Manager, or Webstore customer).");
        }

        string? temporaryPassword = null;
        ApplicationUser user;
        var isCreate = string.IsNullOrWhiteSpace(dto.Id);

        if (isCreate)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email.Trim());
            if (existing is not null)
            {
                return Fail("A user with this email already exists.");
            }

            var password = string.IsNullOrWhiteSpace(dto.Password)
                ? IdentityPasswordGenerator.GenerateTemporaryPassword()
                : dto.Password;

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                temporaryPassword = password;
            }

            user = new ApplicationUser
            {
                UserName = dto.Email.Trim(),
                Email = dto.Email.Trim(),
                EmailConfirmed = true,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                LockoutEnabled = dto.LockoutEnabled,
                CustomerId = dto.IsCustomer ? dto.CustomerId : null
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                return Fail(createResult.Errors.Select(e => e.Description).ToArray());
            }
        }
        else
        {
            var existingUser = await _userManager.FindByIdAsync(dto.Id);
            if (existingUser is null)
            {
                return Fail("User not found.");
            }

            user = existingUser;
            user.FirstName = dto.FirstName.Trim();
            user.LastName = dto.LastName.Trim();
            user.PhoneNumber = dto.PhoneNumber?.Trim();
            user.LockoutEnabled = dto.LockoutEnabled;

            if (dto.IsCustomer)
            {
                user.CustomerId = dto.CustomerId;
            }
            else if (dto.IsAdmin || dto.IsManager)
            {
                user.CustomerId = null;
            }

            if (!string.Equals(user.Email, dto.Email.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var setEmail = await _userManager.SetEmailAsync(user, dto.Email.Trim());
                if (!setEmail.Succeeded)
                {
                    return Fail(setEmail.Errors.Select(e => e.Description).ToArray());
                }

                await _userManager.SetUserNameAsync(user, dto.Email.Trim());
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Fail(updateResult.Errors.Select(e => e.Description).ToArray());
            }
        }

        await SyncRolesAsync(user, dto.IsAdmin, dto.IsManager, dto.IsCustomer);

        if (dto.IsCustomer && dto.CustomerId is > 0)
        {
            await _storeCustomers.LinkIdentityUserToCustomerAsync(user.Id, dto.CustomerId.Value, cancellationToken);
            user.CustomerId = dto.CustomerId;
            await _userManager.UpdateAsync(user);
        }

        await ApplyLockoutAsync(user, dto.IsActive);

        await AuditManualLogger.LogIdentityUserAsync(
            _audit,
            isCreate ? AuditActions.Create : AuditActions.Update,
            user.Id,
            user.Email,
            new
            {
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                isAdmin = dto.IsAdmin,
                isManager = dto.IsManager,
                isCustomer = dto.IsCustomer,
                isActive = dto.IsActive,
                customerId = user.CustomerId
            },
            cancellationToken);

        return new ApplicationUserAccountSaveResult
        {
            Succeeded = true,
            UserId = user.Id,
            TemporaryPassword = temporaryPassword
        };
    }

    public async Task<PasswordResetResult> ResetPasswordAsync(
        string userId,
        string? newPassword = null,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new PasswordResetResult { Succeeded = false, Errors = ["User not found."] };
        }

        return await _passwordPort.ResetPasswordAsync(userId, newPassword, cancellationToken);
    }

    private async Task SyncRolesAsync(ApplicationUser user, bool isAdmin, bool isManager, bool isCustomer)
    {
        var current = await _userManager.GetRolesAsync(user);
        var desired = new List<string>();
        if (isAdmin)
        {
            desired.Add(AppRoles.Admin);
        }

        if (isManager)
        {
            desired.Add(AppRoles.Manager);
        }

        if (isCustomer)
        {
            desired.Add(AppRoles.Customer);
        }

        foreach (var role in current.Except(desired))
        {
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        foreach (var role in desired.Except(current))
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }

    private async Task ApplyLockoutAsync(ApplicationUser user, bool isActive)
    {
        if (isActive)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
            return;
        }

        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
    }

    private static ApplicationUserAccountDto MapDto(ApplicationUser user, IList<string> roles) =>
        new()
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = roles.Contains(AppRoles.Admin),
            IsManager = roles.Contains(AppRoles.Manager),
            IsCustomer = roles.Contains(AppRoles.Customer),
            IsActive = user.LockoutEnd is null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            CustomerId = user.CustomerId,
            AccountType = FormatAccountType(roles)
        };

    private static ApplicationUserAccountEditDto MapEditDto(ApplicationUser user, IList<string> roles) =>
        new()
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsAdmin = roles.Contains(AppRoles.Admin),
            IsManager = roles.Contains(AppRoles.Manager),
            IsCustomer = roles.Contains(AppRoles.Customer),
            IsActive = user.LockoutEnd is null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            LockoutEnabled = user.LockoutEnabled,
            CustomerId = user.CustomerId
        };

    private static string FormatAccountType(IList<string> roles)
    {
        var parts = new List<string>();
        if (roles.Contains(AppRoles.Admin) || roles.Contains(AppRoles.Manager))
        {
            parts.Add("Admin");
        }

        if (roles.Contains(AppRoles.Customer))
        {
            parts.Add("Webstore");
        }

        return parts.Count == 0 ? "—" : string.Join(" + ", parts);
    }

    private static ApplicationUserAccountSaveResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
