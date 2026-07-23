using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.UseCases.Admin;

namespace WebShopABMATIC.Tests.Unit.Admin;

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
