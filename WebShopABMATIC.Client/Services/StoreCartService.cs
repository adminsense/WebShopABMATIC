using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Web.Services;

public sealed class StoreCartService
{
    private const string LegacyStorageKey = "store-cart-v1";
    private const string GuestOwnerKey = "guest";

    private readonly IStoreCatalogPort _catalog;
    private readonly ProtectedLocalStorage _storage;
    private readonly List<CartLine> _lines = [];
    private readonly SemaphoreSlim _loadGate = new(1, 1);

    private string _ownerKey = GuestOwnerKey;
    private bool _loaded;
    private int? _customerId;

    public StoreCartService(IStoreCatalogPort catalog, ProtectedLocalStorage storage)
    {
        _catalog = catalog;
        _storage = storage;
    }

    public event Action? Changed;

    public IReadOnlyList<CartLine> Lines => _lines;

    public int ItemCount => IsCustomerCart ? _lines.Sum(l => l.Quantity) : 0;

    public bool IsCustomerCart => _customerId is > 0;

    public int? CustomerId => _customerId;

    public decimal Subtotal => IsCustomerCart ? _lines.Sum(l => l.LineTotal) : 0m;

    public decimal DeliveryFee => IsCustomerCart && _lines.Count > 0 ? 9.00m : 0m;

    public decimal VatAmount => Math.Round((Subtotal + DeliveryFee) * 0.21m, 2);

    public decimal Total => Subtotal + DeliveryFee + VatAmount;

    public static string ProductImageUrl(int productId) => $"/api/store/products/{productId}/image";

    private string StorageKey => $"store-cart-v1:{_ownerKey}";

    /// <summary>
    /// Bind the cart to the authenticated customer (or clear it for guests).
    /// Only that customer's stored lines are visible — never another user's cart.
    /// </summary>
    public async Task BindToCustomerAsync(int? customerId, CancellationToken cancellationToken = default)
    {
        var nextOwner = customerId is > 0 ? $"c{customerId.Value}" : GuestOwnerKey;
        if (_loaded && _ownerKey == nextOwner && _customerId == customerId)
        {
            return;
        }

        await _loadGate.WaitAsync(cancellationToken);
        try
        {
            _customerId = customerId is > 0 ? customerId : null;
            _ownerKey = nextOwner;
            _lines.Clear();
            _loaded = false;
        }
        finally
        {
            _loadGate.Release();
        }

        if (_customerId is > 0)
        {
            await EnsureLoadedAsync(cancellationToken);
        }
        else
        {
            Changed?.Invoke();
        }
    }

    public async Task EnsureLoadedAsync(CancellationToken cancellationToken = default)
    {
        // Guests never load another user's (or shared legacy) cart into memory.
        if (_customerId is null or <= 0)
        {
            if (_lines.Count > 0 || _loaded)
            {
                _lines.Clear();
                _loaded = true;
                Changed?.Invoke();
            }
            else
            {
                _loaded = true;
            }

            return;
        }

        if (_loaded)
        {
            return;
        }

        await _loadGate.WaitAsync(cancellationToken);
        try
        {
            if (_loaded)
            {
                return;
            }

            try
            {
                var result = await _storage.GetAsync<List<CartLine>>(StorageKey);
                if (result.Success && result.Value is { Count: > 0 })
                {
                    ApplyLines(result.Value);
                }
                else
                {
                    // One-time migrate from the old shared key into this customer's cart only.
                    var legacy = await _storage.GetAsync<List<CartLine>>(LegacyStorageKey);
                    if (legacy.Success && legacy.Value is { Count: > 0 })
                    {
                        ApplyLines(legacy.Value);
                        await _storage.SetAsync(StorageKey, _lines.ToList());
                        await _storage.DeleteAsync(LegacyStorageKey);
                    }
                }

                _loaded = true;
                Changed?.Invoke();
            }
            catch (InvalidOperationException)
            {
                // Browser storage not ready yet — retry on next interactive render.
            }
        }
        finally
        {
            _loadGate.Release();
        }
    }

    public async Task AddProductAsync(
        int productId,
        int quantity = 1,
        IReadOnlyList<CartLineOption>? options = null,
        CancellationToken cancellationToken = default)
    {
        if (_customerId is null or <= 0)
        {
            return;
        }

        await EnsureLoadedAsync(cancellationToken);
        if (!_loaded)
        {
            return;
        }

        var dto = await _catalog.GetByIdAsync(productId, cancellationToken);
        if (dto is null)
        {
            return;
        }

        var product = StoreProductMapper.ToModel(dto);
        if (!product.HasPrice)
        {
            return;
        }

        var selected = (options ?? []).ToList();
        var candidate = new CartLine
        {
            ProductId = product.Id,
            Name = product.Name,
            ImageUrl = ProductImageUrl(product.Id),
            UnitPrice = product.Price!.Value,
            Quantity = quantity,
            Options = selected
        };

        var line = _lines.FirstOrDefault(l => l.Signature == candidate.Signature);
        if (line is null)
        {
            _lines.Add(candidate);
        }
        else
        {
            line.Quantity = Math.Min(line.Quantity + quantity, Math.Max(product.Stock, 1));
            line.ImageUrl = ProductImageUrl(product.Id);
        }

        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    public async Task SetQuantityAsync(string lineId, int quantity, CancellationToken cancellationToken = default)
    {
        if (_customerId is null or <= 0)
        {
            return;
        }

        await EnsureLoadedAsync(cancellationToken);
        if (!_loaded)
        {
            return;
        }

        var line = _lines.FirstOrDefault(l => l.Id == lineId);
        if (line is null)
        {
            return;
        }

        if (quantity <= 0)
        {
            _lines.Remove(line);
        }
        else
        {
            line.Quantity = quantity;
        }

        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    public async Task RemoveAsync(string lineId, CancellationToken cancellationToken = default)
    {
        if (_customerId is null or <= 0)
        {
            return;
        }

        await EnsureLoadedAsync(cancellationToken);
        if (!_loaded)
        {
            return;
        }

        _lines.RemoveAll(l => l.Id == lineId);
        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        if (_customerId is null or <= 0)
        {
            _lines.Clear();
            Changed?.Invoke();
            return;
        }

        await EnsureLoadedAsync(cancellationToken);
        if (!_loaded)
        {
            return;
        }

        _lines.Clear();
        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    private void ApplyLines(IEnumerable<CartLine> source)
    {
        _lines.Clear();
        foreach (var line in source)
        {
            line.ImageUrl = ProductImageUrl(line.ProductId);
            if (string.IsNullOrWhiteSpace(line.Id))
            {
                line.Id = Guid.NewGuid().ToString("N");
            }

            line.Options ??= [];
            _lines.Add(line);
        }
    }

    private async Task PersistAsync(CancellationToken cancellationToken)
    {
        if (_customerId is null or <= 0)
        {
            return;
        }

        try
        {
            if (_lines.Count == 0)
            {
                await _storage.DeleteAsync(StorageKey);
            }
            else
            {
                await _storage.SetAsync(StorageKey, _lines.ToList());
            }
        }
        catch (InvalidOperationException)
        {
            // Browser storage is not available during static/prerender.
        }
    }
}
