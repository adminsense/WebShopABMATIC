using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Seeding;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeed");
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var domainDb = scope.ServiceProvider.GetRequiredService<WebShopABMATICDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await EnsureUserAsync(userManager, domainDb, logger, cancellationToken,
            email: "admin@webshop.com",
            password: "Admin@12345",
            firstName: "Anna",
            lastName: "Rodriguez",
            roles: [AppRoles.Admin, AppRoles.Manager]);

        await EnsureUserAsync(userManager, domainDb, logger, cancellationToken,
            email: "manager@webshop.com",
            password: "Manager@12345",
            firstName: "John",
            lastName: "Sales",
            roles: [AppRoles.Manager]);

        await EnsureUserAsync(userManager, domainDb, logger, cancellationToken,
            email: "customer@webshop.com",
            password: "Customer@12345",
            firstName: "Mary",
            lastName: "Smith",
            roles: [AppRoles.Customer],
            linkCustomerByWebshopLogin: true);

        await AuditLogSeed.SeedAsync(services, cancellationToken);
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        WebShopABMATICDbContext domainDb,
        ILogger logger,
        CancellationToken cancellationToken,
        string email,
        string password,
        string firstName,
        string lastName,
        string[] roles,
        bool linkCustomerByWebshopLogin = false)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                logger.LogWarning("Failed to create seed user {Email}: {Errors}",
                    email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return;
            }

            await userManager.AddToRolesAsync(user, roles);
            logger.LogInformation("Seed user created: {Email}", email);
        }

        if (linkCustomerByWebshopLogin)
        {
            await LinkCustomerAccountAsync(userManager, domainDb, user, email, cancellationToken);
        }
    }

    internal static async Task LinkCustomerAccountAsync(
        UserManager<ApplicationUser> userManager,
        WebShopABMATICDbContext domainDb,
        ApplicationUser user,
        string email,
        CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLowerInvariant();
        var customer = await domainDb.Customers
            .FirstOrDefaultAsync(c =>
                (c.WebshopLogin != null && c.WebshopLogin.ToLower() == normalized) ||
                c.CustomerEmail.ToLower() == normalized, cancellationToken);

        if (customer is null)
        {
            return;
        }

        customer.IdentityUserId = user.Id;
        user.CustomerId = customer.CustomerId;
        await domainDb.SaveChangesAsync(cancellationToken);
        await userManager.UpdateAsync(user);
    }
}
