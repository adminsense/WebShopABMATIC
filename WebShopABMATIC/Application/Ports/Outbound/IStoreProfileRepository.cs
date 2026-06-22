using WebShopABMATIC.Application.Store.Profile;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStoreProfileRepository
{
    Task<StoreProfileDto?> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);

    Task<StoreProfileSaveResult> SaveByCustomerIdAsync(
        int customerId,
        StoreProfileUpdateDto profile,
        CancellationToken cancellationToken = default);
}
