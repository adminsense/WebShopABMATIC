using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace WebShopABMATIC.Web.Services;

public sealed class ProtectedStoreCartSessionStore(ProtectedSessionStorage session) : IStoreCartSessionStore
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var result = await session.GetAsync<T>(key);
        return result.Success ? result.Value : default;
    }

    public async Task SetAsync<T>(string key, T value) => await session.SetAsync(key, value);

    public async Task DeleteAsync(string key) => await session.DeleteAsync(key);
}
