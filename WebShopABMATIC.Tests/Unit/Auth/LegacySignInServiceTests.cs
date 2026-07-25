using System.Security.Claims;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Auth;

namespace WebShopABMATIC.Tests.Unit.Auth;

public sealed class LegacySignInServiceTests
{
    [Fact]
    public async Task SignInStaff_succeeds_for_admin()
    {
        await using var db = CreateDb();
        db.StaffUsers.Add(new StaffUser
        {
            Id = 1,
            Login = "admin",
            Password = "secret",
            FirstName = "Ann",
            LastName = "Admin",
            Admin = true,
            Address = "",
            HiredAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var result = await new LegacySignInService(db).SignInStaffAsync("admin", "secret");

        result.Succeeded.Should().BeTrue();
        result.Principal!.IsInRole(AppRoles.Admin).Should().BeTrue();
        result.Principal.FindFirstValue(LegacyAuthClaims.StaffUserId).Should().Be("1");
    }

    [Fact]
    public async Task SignInStaff_fails_without_roles()
    {
        await using var db = CreateDb();
        db.StaffUsers.Add(new StaffUser
        {
            Id = 2,
            Login = "norole",
            Password = "x",
            FirstName = "N",
            LastName = "R",
            Admin = false,
            Address = "",
            HiredAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var result = await new LegacySignInService(db).SignInStaffAsync("norole", "x");

        result.Succeeded.Should().BeFalse();
        result.Error.Should().Contain("not authorized");
    }

    [Fact]
    public async Task SignInCustomer_succeeds_with_hashed_password()
    {
        var (hash, salt) = LegacyWebshopPasswordVerifier.CreateHash("Passw0rd!");
        await using var db = CreateDb();
        db.Customers.Add(CreateCustomer(10, "shopuser", "user@test.com", hash, salt));
        await db.SaveChangesAsync();

        var result = await new LegacySignInService(db).SignInCustomerAsync("shopuser", "Passw0rd!");

        result.Succeeded.Should().BeTrue();
        result.Principal!.IsInRole(AppRoles.Customer).Should().BeTrue();
        result.Principal.FindFirstValue(LegacyAuthClaims.CustomerId).Should().Be("10");
    }

    [Fact]
    public async Task SignInCustomer_fails_bad_password()
    {
        var (hash, salt) = LegacyWebshopPasswordVerifier.CreateHash("Passw0rd!");
        await using var db = CreateDb();
        db.Customers.Add(CreateCustomer(11, "shopuser", "u@test.com", hash, salt));
        await db.SaveChangesAsync();

        var result = await new LegacySignInService(db).SignInCustomerAsync("shopuser", "wrong");

        result.Succeeded.Should().BeFalse();
    }

    private static WebShopABMATICDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<WebShopABMATICDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new WebShopABMATICDbContext(options);
    }

    private static Customer CreateCustomer(
        int id,
        string login,
        string email,
        string hash,
        string salt) =>
        new()
        {
            CustomerId = id,
            WebshopLogin = login,
            CustomerEmail = email,
            WebshopPasswordHash = hash,
            WebshopPasswordSalt = salt,
            CustomerName = "Test",
            CustomerVatNumber = "",
            CustomerBox = "",
            CustomerHouseNumber = "",
            CustomerStreet = "",
            CustomerPhone = "",
            CustomerFax = "",
            LockedBy = "",
            CustomerGroup = ""
        };
}
