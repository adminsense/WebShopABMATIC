using WebShopABMATIC.Application.Store.Profile;

namespace WebShopABMATIC.Application.Ports;

public interface IStoreProfilePort
{
    Task<StoreProfileDto?> GetMyProfileAsync(CancellationToken cancellationToken = default);

    Task<StoreProfileSaveResult> SaveMyProfileAsync(
        StoreProfileUpdateDto profile,
        CancellationToken cancellationToken = default);
}
