using WebShopABMATIC.Application.Store.Checkout;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStoreCustomerRepository
{
    Task<StoreCustomerContext?> GetForStoreUserAsync(StoreUserLookup lookup, CancellationToken cancellationToken = default);

    Task LinkIdentityUserToCustomerAsync(string identityUserId, int customerId, CancellationToken cancellationToken = default);
}

public sealed class StoreUserLookup
{
    public string? IdentityUserId { get; init; }
    public string? Email { get; init; }
}

public sealed class StoreCustomerContext
{
    public int CustomerId { get; init; }
    public int CustomerTypeId { get; init; }
    public int DeliveryTypeId { get; init; }
    public int BetaaltermijnId { get; init; }
    public int ProjectId { get; init; }
}
