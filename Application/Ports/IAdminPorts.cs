using WebShopABMATIC.Application.Admin.Customers;
using WebShopABMATIC.Application.Admin.Dashboard;
using WebShopABMATIC.Application.Admin.Hubs;
using WebShopABMATIC.Application.Admin.Orders;
using WebShopABMATIC.Application.Admin.Products;
using WebShopABMATIC.Application.Common;

namespace WebShopABMATIC.Application.Ports;

public interface IAdminDashboardPort
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
}

public interface IAdminHubPort
{
    IReadOnlyList<AdminHubDefinitionDto> GetHubDefinitions();
    AdminHubDefinitionDto? GetHub(string hubId);
}

public interface IProductAdminPort
{
    Task<PagedResult<ProductDto>> GetProductsAsync(ProductListFilter filter, CancellationToken cancellationToken = default);
    Task<ProductEditDto?> GetForEditAsync(int productId, CancellationToken cancellationToken = default);
    Task<int> SaveAsync(ProductEditDto dto, CancellationToken cancellationToken = default);
}

public interface ICustomerAdminPort
{
    Task<PagedResult<CustomerDto>> GetCustomersAsync(CustomerListFilter filter, CancellationToken cancellationToken = default);
}

public interface IOrderAdminPort
{
    Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(OrderListFilter filter, CancellationToken cancellationToken = default);
}
