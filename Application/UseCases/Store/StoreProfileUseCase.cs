using WebShopABMATIC.Application.Auth;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Ports.Outbound;
using WebShopABMATIC.Application.Store.Profile;

namespace WebShopABMATIC.Application.UseCases.Store;

public sealed class StoreProfileUseCase : IStoreProfilePort
{
    private readonly IStoreProfileRepository _repository;
    private readonly ICurrentUserContext _currentUser;

    public StoreProfileUseCase(IStoreProfileRepository repository, ICurrentUserContext currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<StoreProfileDto?> GetMyProfileAsync(CancellationToken cancellationToken = default)
    {
        var current = await _currentUser.GetCurrentUserAsync(cancellationToken);
        if (current.CustomerId is not > 0)
        {
            return null;
        }

        return await _repository.GetByCustomerIdAsync(current.CustomerId.Value, cancellationToken);
    }

    public async Task<StoreProfileSaveResult> SaveMyProfileAsync(
        StoreProfileUpdateDto profile,
        CancellationToken cancellationToken = default)
    {
        var current = await _currentUser.GetCurrentUserAsync(cancellationToken);
        if (current.CustomerId is not > 0)
        {
            return new StoreProfileSaveResult { Succeeded = false, Errors = ["Not signed in."] };
        }

        return await _repository.SaveByCustomerIdAsync(current.CustomerId.Value, profile, cancellationToken);
    }
}
