using WebShopABMATIC.Web.Services;

namespace WebShopABMATIC.Tests.TestDoubles;

public sealed class InMemoryStoreCartSessionStore : IStoreCartSessionStore
{
    private readonly Dictionary<string, object?> _bag = new(StringComparer.Ordinal);

    public Task<T?> GetAsync<T>(string key)
    {
        if (_bag.TryGetValue(key, out var value) && value is T typed)
        {
            return Task.FromResult<T?>(typed);
        }

        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value)
    {
        _bag[key] = value;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string key)
    {
        _bag.Remove(key);
        return Task.CompletedTask;
    }

    public bool Contains(string key) => _bag.ContainsKey(key);
}
