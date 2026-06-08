using WebShopABMATIC.Application.Store.Profile;

namespace WebShopABMATIC.Application.Ports.Outbound;

public interface IStoreProfileRepository
{
    Task<StoreProfileDto?> GetAsync(string identityUserId, CancellationToken cancellationToken = default);

    Task<StoreProfileSaveResult> SaveAsync(
        string identityUserId,
        StoreProfileUpdateDto profile,
        CancellationToken cancellationToken = default);
}
