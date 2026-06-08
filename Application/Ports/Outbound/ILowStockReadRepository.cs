using WebShopABMATIC.Application.Admin.Dashboard;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ILowStockReadRepository
{
    Task<IReadOnlyList<LowStockProductDto>> GetLowStockProductsAsync(int limit, CancellationToken cancellationToken = default);
}
