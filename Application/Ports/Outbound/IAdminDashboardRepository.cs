using WebShopABMATIC.Application.Admin.Dashboard;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IAdminDashboardRepository
{
    Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
}
