using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Infrastructure.Identity;

namespace WebShopABMATIC.Infrastructure.Seeding;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeed");
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync(cancellationToken);

        foreach (var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await EnsureUserAsync(userManager, logger,
            email: "admin@webshop.com",
            password: "Admin@12345",
            firstName: "Anna",
            lastName: "Rodriguez",
            roles: [AppRoles.Admin, AppRoles.Manager]);

        await EnsureUserAsync(userManager, logger,
            email: "manager@webshop.com",
            password: "Manager@12345",
            firstName: "John",
            lastName: "Sales",
            roles: [AppRoles.Manager]);

        await EnsureUserAsync(userManager, logger,
            email: "customer@webshop.com",
            password: "Customer@12345",
            firstName: "Mary",
            lastName: "Smith",
            roles: [AppRoles.Customer]);
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        ILogger logger,
        string email,
        string password,
        string firstName,
        string lastName,
        string[] roles)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            return;
        }

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
}

public sealed class IdentitySeedHostedService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly IHostEnvironment _environment;

    public IdentitySeedHostedService(IServiceProvider services, IHostEnvironment environment)
    {
        _services = services;
        _environment = environment;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
        {
            return;
        }

        await IdentitySeed.SeedAsync(_services, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
