using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Data.Entities;
using WebShopABMATIC.Data.Persistence;
using WebShopABMATIC.Infrastructure.Audit;
using WebShopABMATIC.Infrastructure.Auth;
using WebShopABMATIC.Infrastructure.Notifications;
using WebShopABMATIC.Infrastructure.Stock;

namespace WebShopABMATIC.Tests.Stock;

internal static class StockMovementServiceTestContext
{
    public static (WebShopABMATICDbContext Db, StockMovementService Service) Create(
        Action<WebShopABMATICDbContext>? seed = null)
    {
        var options = new DbContextOptionsBuilder<WebShopABMATICDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var db = new WebShopABMATICDbContext(options);
        seed?.Invoke(db);
        db.SaveChanges();

        var service = new StockMovementService(
            db,
            new NullLowStockAlertService(),
            new NullAuditService(),
            new AuditSuppressionContext(),
            NullLogger<StockMovementService>.Instance);

        return (db, service);
    }

    public static void SeedTransferScenario(WebShopABMATICDbContext db)
    {
        db.Products.Add(new Product
        {
            ProductId = 100,
            NameNl = "Widget",
            NameEn = "Widget",
            NameFr = "Widget",
            DescriptionNl = "Test",
            DescriptionEn = "Test",
            DescriptionFr = "Test",
            ShortNotesEn = string.Empty,
            ShortNotesFr = string.Empty,
            WebshopDescriptionNl = string.Empty,
            OrderPartNumber = "W-100",
            StockNumber = "W-100",
            SupplierId = 1,
            ManufacturerId = 1
        });

        db.StockLocations.AddRange(
            new StockLocation { Id = 1, Name = "Warehouse A" },
            new StockLocation { Id = 2, Name = "Warehouse B" });

        db.ProductStockLocations.AddRange(
            new ProductStockLocation
            {
                Id = 10,
                ProductId = 100,
                StockLocationId = 1,
                Quantity = 50m,
                IsInactive = false
            },
            new ProductStockLocation
            {
                Id = 11,
                ProductId = 100,
                StockLocationId = 2,
                Quantity = 5m,
                IsInactive = false
            });
    }
}
