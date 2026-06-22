using WebShopABMATIC.Application.Auth;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface ICurrentUserContext
{
    Task<CurrentUserSnapshot> GetCurrentUserAsync(CancellationToken cancellationToken = default);
}
