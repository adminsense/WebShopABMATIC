namespace WebShopABMATIC.Web.Services;

/// <summary>Abstraction over browser session storage for the store cart (testable).</summary>
public interface IStoreCartSessionStore
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);
    Task DeleteAsync(string key);
}
