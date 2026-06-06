using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Registration;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Identity;
using WebShopABMATIC.Infrastructure.Persistence;

namespace WebShopABMATIC.Infrastructure.Persistence.Repositories;

public sealed class CustomerRegistrationRepository : ICustomerRegistrationRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly WebShopABMATICDbContext _db;

    public CustomerRegistrationRepository(UserManager<ApplicationUser> userManager, WebShopABMATICDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public async Task<CustomerRegistrationResult> RegisterAsync(CustomerRegistrationRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim();
        var normalized = email.ToLowerInvariant();

        if (await _userManager.FindByEmailAsync(email) is not null)
        {
            return Fail("An account with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim()
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            return Fail(createResult.Errors.Select(e => e.Description).ToArray());
        }

        var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.Customer);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return Fail(roleResult.Errors.Select(e => e.Description).ToArray());
        }

        var customerName = string.IsNullOrWhiteSpace(request.CompanyName)
            ? $"{user.FirstName} {user.LastName}".Trim()
            : request.CompanyName.Trim();

        var customer = await _db.Customers
            .FirstOrDefaultAsync(c =>
                (c.WebshopLogin != null && c.WebshopLogin.ToLower() == normalized) ||
                c.CustomerEmail.ToLower() == normalized, cancellationToken);

        if (customer is not null)
        {
            if (!string.IsNullOrWhiteSpace(customer.IdentityUserId))
            {
                await _userManager.DeleteAsync(user);
                return Fail("This email is already linked to a webshop account.");
            }

            customer.IdentityUserId = user.Id;
            customer.WebshopLogin ??= email;
            if (string.IsNullOrWhiteSpace(customer.CustomerEmail))
            {
                customer.CustomerEmail = email;
            }
        }
        else
        {
            customer = (Customer)AdminCrudDefaults.Create("customers");
            customer.CustomerName = customerName;
            customer.CustomerEmail = email;
            customer.WebshopLogin = email;
            customer.FirstContactName = $"{user.FirstName} {user.LastName}".Trim();
            customer.IdentityUserId = user.Id;
            _db.Customers.Add(customer);
        }

        await _db.SaveChangesAsync(cancellationToken);

        user.CustomerId = customer.CustomerId;
        await _userManager.UpdateAsync(user);

        await EnsureProjectAsync(customer, customerName, cancellationToken);

        return new CustomerRegistrationResult
        {
            Succeeded = true,
            IdentityUserId = user.Id,
            CustomerId = customer.CustomerId
        };
    }

    private async Task EnsureProjectAsync(Customer customer, string customerName, CancellationToken cancellationToken)
    {
        var hasProject = await _db.Projects.AnyAsync(p => p.CustomerId == customer.CustomerId, cancellationToken);
        if (hasProject)
        {
            return;
        }

        var nextNumber = await _db.Projects.MaxAsync(p => (int?)p.ProjectNumber, cancellationToken) ?? 1000;
        nextNumber++;

        _db.Projects.Add(new Project
        {
            ProjectNumber = nextNumber,
            ProjectName = $"Webshop — {customerName}",
            ProjectManagerUserId = 1,
            CustomerId = customer.CustomerId,
            ProjectTypeId = 1,
            IsStandardProject = true
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static CustomerRegistrationResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
