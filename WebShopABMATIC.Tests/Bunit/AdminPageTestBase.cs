using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Admin.Hubs;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Admin.Stock;
using WebShopABMATIC.Application.Common;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Checkout;
using WebShopABMATIC.Application.Store.Profile;

namespace WebShopABMATIC.Tests.Bunit;

/// <summary>DI stubs shared by Admin + customer-auth Store page tests.</summary>
public abstract class AdminPageTestBase : BunitStoreTestBase
{
    protected ILegacyStaffProfilePort StaffProfile { get; } = Substitute.For<ILegacyStaffProfilePort>();
    protected IStoreProfilePort StoreProfile { get; } = Substitute.For<IStoreProfilePort>();
    protected IStockAdjustmentPort StockAdjustment { get; } = Substitute.For<IStockAdjustmentPort>();
    protected IStockTransferPort StockTransfer { get; } = Substitute.For<IStockTransferPort>();
    protected IStockOverviewPort StockOverview { get; } = Substitute.For<IStockOverviewPort>();
    protected IAdminHubPort HubPort { get; } = Substitute.For<IAdminHubPort>();

    protected AdminPageTestBase()
    {
        Services.AddSingleton(Substitute.For<ICustomerAdminPort>());
        Services.AddSingleton(Substitute.For<IOrderAdminPort>());
        Services.AddSingleton(StockOverview);
        Services.AddSingleton(StockAdjustment);
        Services.AddSingleton(StockTransfer);
        Services.AddSingleton(Substitute.For<IStockOrderAdminPort>());
        Services.AddSingleton(Substitute.For<IStockPoReceivePort>());
        Services.AddSingleton(Substitute.For<IStockMovementAdminPort>());
        Services.AddSingleton(Substitute.For<IStockLocationAdminPort>());
        Services.AddSingleton(Substitute.For<IProductStockLocationAdminPort>());
        Services.AddSingleton(Substitute.For<IProductPriceAdminPort>());
        Services.AddSingleton(Substitute.For<IProductOptionAdminPort>());
        Services.AddSingleton(Substitute.For<IProductQuantityTierAdminPort>());
        Services.AddSingleton(Substitute.For<IManufacturerAdminPort>());
        Services.AddSingleton(Substitute.For<ISupplierAdminPort>());
        Services.AddSingleton(Substitute.For<IPriceListCategoryAdminPort>());
        Services.AddSingleton(Substitute.For<IWebshopStructureAdminPort>());
        Services.AddSingleton(Substitute.For<IWebshopProductStructureAdminPort>());
        Services.AddSingleton(Substitute.For<ICustomerTypeAdminPort>());
        Services.AddSingleton(Substitute.For<ICustomerDeliveryAddressAdminPort>());
        Services.AddSingleton(Substitute.For<ICustomerProductDiscountAdminPort>());
        Services.AddSingleton(Substitute.For<IDeliveryTypeAdminPort>());
        Services.AddSingleton(Substitute.For<IPaymentMethodAdminPort>());
        Services.AddSingleton(Substitute.For<IOrderStatusAdminPort>());
        Services.AddSingleton(Substitute.For<IVatTypeAdminPort>());
        Services.AddSingleton(Substitute.For<IUserGroupAdminPort>());
        Services.AddSingleton(Substitute.For<IStaffUserAdminPort>());
        Services.AddSingleton(Substitute.For<IAuditLogAdminPort>());
        Services.AddSingleton(HubPort);
        Services.AddSingleton(StoreProfile);
        Services.AddSingleton(Substitute.For<ICustomerRegistrationPort>());
        Services.AddSingleton(Substitute.For<ILegacyCustomerPasswordPort>());
        Services.AddSingleton(StaffProfile);
        Services.AddSingleton(Substitute.For<IMollieWebhookPort>());

        ProductAdmin.GetProductsAsync(Arg.Any<ProductListFilter>(), Arg.Any<CancellationToken>())
            .Returns(new PagedResult<ProductDto>
            {
                Items = [],
                TotalCount = 0,
                Page = 1,
                PageSize = 20
            });
        Dashboard.GetDashboardAsync(Arg.Any<CancellationToken>())
            .Returns(new AdminDashboardDto { TotalProducts = 1, ProductsOnWebshop = 1 });

        Catalog.GetCategoryTreeAsync(Arg.Any<CancellationToken>()).Returns([]);
        Catalog.GetDealsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns([]);
        Catalog.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((Application.Store.StoreProductDto?)null);
        Catalog.GetProductOptionsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns([]);

        Checkout.GetCustomerOrdersAsync(Arg.Any<StoreUserLookup>(), Arg.Any<CancellationToken>()).Returns([]);
        Checkout.GetOrderSummaryAsync(Arg.Any<int>(), Arg.Any<StoreUserLookup>(), Arg.Any<CancellationToken>())
            .Returns((CheckoutOrderSummaryDto?)null);

        StoreProfile.GetMyProfileAsync(Arg.Any<CancellationToken>())
            .Returns(new StoreProfileDto
            {
                CustomerId = 10,
                Email = "buyer@test.local",
                FirstName = "A",
                LastName = "B"
            });

        StaffProfile.GetAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(new LegacyStaffProfileDto
            {
                StaffUserId = 1,
                Login = "staff",
                Email = "staff@test.local",
                FirstName = "Staff",
                LastName = "User"
            });

        StockAdjustment.GetLookupsAsync(Arg.Any<CancellationToken>())
            .Returns(new StockAdjustmentLookupsDto());
        StockTransfer.GetLookupsAsync(Arg.Any<CancellationToken>())
            .Returns(new StockTransferLookupsDto());
        StockOverview.GetOverviewAsync(Arg.Any<CancellationToken>())
            .Returns(new StockOverviewDto());

        HubPort.GetHub(Arg.Any<string>()).Returns(new AdminHubDefinitionDto
        {
            Id = "catalog",
            Title = "Catalog",
            Subtitle = "Manage catalog",
            IconClass = "oi-box",
            Cards = []
        });
    }
}
