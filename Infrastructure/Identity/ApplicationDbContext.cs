using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebShopABMATIC.Infrastructure.Identity;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<StockLowAlert> StockLowAlerts => Set<StockLowAlert>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(entity => entity.Property(u => u.CustomerId));

        builder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Action).HasMaxLength(50).IsRequired();
            entity.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.EntityId).HasMaxLength(50);
            entity.Property(x => x.IdentityUserId).HasMaxLength(450);
            entity.Property(x => x.UserDisplayName).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Severity).HasMaxLength(20).IsRequired();
            entity.Property(x => x.IpAddress).HasMaxLength(45);
            entity.Property(x => x.UserAgent).HasMaxLength(512);
            entity.HasIndex(x => x.Timestamp);
            entity.HasIndex(x => new { x.IdentityUserId, x.Timestamp });
            entity.HasIndex(x => new { x.EntityName, x.EntityId });
            entity.HasIndex(x => new { x.Action, x.Success });
        });

        builder.Entity<StockLowAlert>(entity =>
        {
            entity.ToTable("StockLowAlerts");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ProductName).HasMaxLength(256).IsRequired();
            entity.Property(x => x.StockLocationName).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Quantity).HasColumnType("decimal(18,4)");
            entity.Property(x => x.MinQuantity).HasColumnType("decimal(18,4)");
            entity.HasIndex(x => x.CreatedAt);
            entity.HasIndex(x => new { x.IsRead, x.CreatedAt });
            entity.HasIndex(x => x.ProductStockLocationId);
        });
    }
}
