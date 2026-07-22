using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Stock;
using WebShopABMATIC.Application.UseCases.Admin;
using WebShopABMATIC.Domain.Catalog.Products;

namespace WebShopABMATIC.Tests.Unit.Admin;

public sealed class ProductAdminUseCaseTests
{
    [Fact]
    public async Task SaveAsync_creates_new_product()
    {
        var repo = Substitute.For<IProductRepository>();
        var media = Substitute.For<IProductMediaPort>();
        var current = Substitute.For<ICurrentUserContext>();
        current.GetCurrentUserAsync(Arg.Any<CancellationToken>())
            .Returns(new CurrentUserSnapshot { IsAuthenticated = true, StaffUserId = 3 });
        repo.SaveAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>()).Returns(55);

        var sut = new ProductAdminUseCase(repo, media, current);
        var id = await sut.SaveAsync(new ProductEditDto
        {
            ProductId = 0,
            NameEn = "Remote",
            OrderPartNumber = "R1",
            SupplierId = 1,
            ManufacturerId = 2,
            ShowOnWebshop = true,
            WebshopDescriptionNl = "desc"
        }, primaryImage: null);

        id.Should().Be(55);
        await repo.Received(1).SaveAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await media.Received(1).SetPrimaryImagePublishToWebAsync(55, true, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductsAsync_delegates_filter()
    {
        var repo = Substitute.For<IProductRepository>();
        var filter = new ProductListFilter { ShowOnWebshop = true };
        repo.GetProductsAsync(filter, Arg.Any<CancellationToken>())
            .Returns(new PagedResult<ProductDto>
            {
                Items = [new ProductDto { ProductId = 1, NameEn = "A", ShowOnWebshop = true }],
                TotalCount = 1,
                Page = 1,
                PageSize = 20
            });

        var page = await new ProductAdminUseCase(repo, Substitute.For<IProductMediaPort>(), Substitute.For<ICurrentUserContext>())
            .GetProductsAsync(filter);

        page.TotalCount.Should().Be(1);
        page.Items[0].NameEn.Should().Be("A");
    }
}

public sealed class OrderAdminUseCaseTests
{
    [Fact]
    public async Task CancelOrder_not_found()
    {
        var repo = Substitute.For<IOrderRepository>();
        repo.GetForEditAsync(99, Arg.Any<CancellationToken>()).Returns((OrderEditDto?)null);

        var result = await new OrderAdminUseCase(
                repo,
                Substitute.For<IStockMovementService>(),
                Substitute.For<IAuditService>(),
                Substitute.For<IAuditLogRepository>())
            .CancelOrderAsync(99, "test");

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");
    }

    [Fact]
    public async Task CancelOrder_releases_reservation_and_saves()
    {
        var repo = Substitute.For<IOrderRepository>();
        var stock = Substitute.For<IStockMovementService>();
        var audit = Substitute.For<IAuditService>();
        var order = new OrderEditDto { Id = 5, IsAccepted = true };
        repo.GetForEditAsync(5, Arg.Any<CancellationToken>()).Returns(order);
        stock.ReleaseReservationAsync(5, Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(2));

        var result = await new OrderAdminUseCase(repo, stock, audit, Substitute.For<IAuditLogRepository>())
            .CancelOrderAsync(5, "customer request");

        result.Success.Should().BeTrue();
        result.ReservationsReleased.Should().Be(2);
        order.IsAccepted.Should().BeFalse();
        await repo.Received(1).SaveAsync(order, Arg.Any<CancellationToken>());
    }
}

public sealed class StockAdjustmentUseCaseTests
{
    [Fact]
    public async Task ApplyAsync_forwards_to_stock_service()
    {
        var stock = Substitute.For<IStockMovementService>();
        stock.ApplyManualAdjustmentAsync(Arg.Any<StockManualAdjustmentCommand>(), Arg.Any<CancellationToken>())
            .Returns(StockApplyResult.Applied(1, movementId: 8, newBalance: 12));

        var sut = new StockAdjustmentUseCase(Substitute.For<IStockAdjustmentRepository>(), stock);
        var result = await sut.ApplyAsync(new StockAdjustmentRequest
        {
            ProductId = 1,
            StockLocationId = 2,
            QuantityChange = 3,
            Reason = "count"
        });

        result.IsSuccess.Should().BeTrue();
        result.MovementId.Should().Be(8);
        await stock.Received(1).ApplyManualAdjustmentAsync(
            Arg.Is<StockManualAdjustmentCommand>(c =>
                c.ProductId == 1 && c.StockLocationId == 2 && c.QuantityChange == 3 && c.Reason == "count"),
            Arg.Any<CancellationToken>());
    }
}

public sealed class AdminDashboardUseCaseTests
{
    [Fact]
    public async Task GetDashboard_merges_core_and_low_stock()
    {
        var repo = Substitute.For<IAdminDashboardRepository>();
        repo.GetDashboardAsync(Arg.Any<CancellationToken>())
            .Returns(new AdminDashboardDto { TotalProducts = 10, ProductsOnWebshop = 4 });
        var lowStock = Substitute.For<ILowStockReadRepository>();
        lowStock.GetLowStockProductsAsync(25, Arg.Any<CancellationToken>())
            .Returns([new LowStockProductDto { ProductId = 1, ProductName = "Low" }]);
        var alerts = Substitute.For<ILowStockAlertService>();
        alerts.GetUnreadAlertsAsync(10, Arg.Any<CancellationToken>()).Returns([]);
        alerts.GetUnreadCountAsync(Arg.Any<CancellationToken>()).Returns(0);

        var dto = await new AdminDashboardUseCase(repo, lowStock, alerts).GetDashboardAsync();

        dto.TotalProducts.Should().Be(10);
        dto.LowStockProducts.Should().ContainSingle(p => p.ProductName == "Low");
    }
}
