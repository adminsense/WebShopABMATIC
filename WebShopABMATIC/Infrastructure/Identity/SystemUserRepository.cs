using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Admin.AuditLogs;
using WebShopABMATIC.Application.Admin.SystemUsers;
using WebShopABMATIC.Application.Audit;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Infrastructure.Audit;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class SystemUserRepository : ISystemUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityPasswordPort _passwordPort;
    private readonly IAuditService _audit;

    public SystemUserRepository(
        UserManager<ApplicationUser> userManager,
        IIdentityPasswordPort passwordPort,
        IAuditService audit)
    {
        _userManager = userManager;
        _passwordPort = passwordPort;
        _audit = audit;
    }

    public async Task<PagedResult<SystemUserDto>> GetSystemUsersAsync(SystemUserListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users.AsNoTracking().Where(u => u.CustomerId == null);

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

        var items = new List<SystemUserDto>();
        foreach (var user in candidates)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(AppRoles.Admin) && !roles.Contains(AppRoles.Manager))
            {
                continue;
            }

            items.Add(MapDto(user, roles));
        }

        var total = items.Count;
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var pageItems = items
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<SystemUserDto>
        {
            Items = pageItems,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<SystemUserEditDto?> GetForEditAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null || user.CustomerId is not null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(AppRoles.Admin) && !roles.Contains(AppRoles.Manager))
        {
            return null;
        }

        return MapEditDto(user, roles);
    }

    public async Task<SystemUserSaveResult> SaveAsync(SystemUserEditDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return Fail("Email is required.");
        }

        if (!dto.IsAdmin && !dto.IsManager)
        {
            return Fail("Select at least one role (Admin or Manager).");
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
                LockoutEnabled = dto.LockoutEnabled
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
            user.LockoutEnabled = dto.LockoutEnabled;

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

        await SyncRolesAsync(user, dto.IsAdmin, dto.IsManager);
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
                isActive = dto.IsActive
            },
            cancellationToken);

        return new SystemUserSaveResult
        {
            Succeeded = true,
            UserId = user.Id,
            TemporaryPassword = temporaryPassword
        };
    }

    public async Task<PasswordResetResult> ResetPasswordAsync(string userId, string? newPassword = null, CancellationToken cancellationToken = default)
    {
        var user = await GetForEditAsync(userId, cancellationToken);
        if (user is null)
        {
            return new PasswordResetResult { Succeeded = false, Errors = ["User not found."] };
        }

        return await _passwordPort.ResetPasswordAsync(userId, newPassword, cancellationToken);
    }

    private async Task SyncRolesAsync(ApplicationUser user, bool isAdmin, bool isManager)
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

        foreach (var role in current.Where(r => r is AppRoles.Admin or AppRoles.Manager).Except(desired))
        {
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        foreach (var role in desired.Except(current))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        if (current.Contains(AppRoles.Customer))
        {
            await _userManager.RemoveFromRoleAsync(user, AppRoles.Customer);
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

    private static SystemUserDto MapDto(ApplicationUser user, IList<string> roles) =>
        new()
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = roles.Contains(AppRoles.Admin),
            IsManager = roles.Contains(AppRoles.Manager),
            IsActive = user.LockoutEnd is null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            LockoutEnabled = user.LockoutEnabled
        };

    private static SystemUserEditDto MapEditDto(ApplicationUser user, IList<string> roles) =>
        new()
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = roles.Contains(AppRoles.Admin),
            IsManager = roles.Contains(AppRoles.Manager),
            IsActive = user.LockoutEnd is null || user.LockoutEnd <= DateTimeOffset.UtcNow,
            LockoutEnabled = user.LockoutEnabled
        };

    private static SystemUserSaveResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
