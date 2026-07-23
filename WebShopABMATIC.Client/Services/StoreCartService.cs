using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Web.Services;

/// <summary>
/// Store cart lives in browser <strong>session</strong> storage only (guest + customer keys).
/// Closing the browser or Sign out clears it — nothing survives a new browser session.
/// ERP stock is not reserved until PrePay place-order.
/// </summary>
public sealed class StoreCartService
{
    private const string GuestSessionKey = "store-cart-v1:guest-session";

    private readonly IStoreCatalogPort _catalog;
    private readonly IStoreCartSessionStore _session;
    private readonly List<CartLine> _lines = [];
    private readonly SemaphoreSlim _loadGate = new(1, 1);

    private string _ownerKey = "guest";
    private bool _loaded;
    private int? _customerId;

    public StoreCartService(IStoreCatalogPort catalog, IStoreCartSessionStore session)
    {
        _catalog = catalog;
        _session = session;
    }

    public event Action? Changed;

    public IReadOnlyList<CartLine> Lines => _lines;

    public int ItemCount => _lines.Sum(l => l.Quantity);

    public bool IsCustomerCart => _customerId is > 0;

    public bool HasLines => _lines.Count > 0;

    public int? CustomerId => _customerId;

    public decimal Subtotal => _lines.Sum(l => l.LineTotal);

    public decimal DeliveryFee => 0m;

    public decimal VatAmount => Math.Round((Subtotal + DeliveryFee) * 0.21m, 2);

    public decimal Total => Subtotal + DeliveryFee + VatAmount;

    public static string ProductImageUrl(int productId) => $"/api/store/products/{productId}/image";

    private string SessionKey => _customerId is > 0 ? $"store-cart-v1:c{_customerId}" : GuestSessionKey;

    /// <summary>
    /// Bind to authenticated customer (merge guest session lines) or guest session cart.
    /// </summary>
    public async Task BindToCustomerAsync(int? customerId, CancellationToken cancellationToken = default)
    {
        var nextOwner = customerId is > 0 ? $"c{customerId.Value}" : "guest";
        if (_loaded && _ownerKey == nextOwner && _customerId == (customerId is > 0 ? customerId : null))
        {
            return;
        }

        List<CartLine>? guestLinesToMerge = null;
        if (customerId is > 0)
        {
            guestLinesToMerge = await ReadSessionLinesAsync(GuestSessionKey);
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

        await EnsureLoadedAsync(cancellationToken);

        if (_customerId is > 0 && guestLinesToMerge is { Count: > 0 })
        {
            await MergeLinesAsync(guestLinesToMerge, cancellationToken);
            await DeleteSessionKeyAsync(GuestSessionKey);
        }
    }

    /// <summary>Clears in-memory cart and all session keys (Sign out).</summary>
    public async Task SignOutClearAsync(CancellationToken cancellationToken = default)
    {
        var customerKey = _customerId is > 0 ? SessionKey : null;

        await _loadGate.WaitAsync(cancellationToken);
        try
        {
            _customerId = null;
            _ownerKey = "guest";
            _lines.Clear();
            _loaded = true;
        }
        finally
        {
            _loadGate.Release();
        }

        await DeleteSessionKeyAsync(GuestSessionKey);
        if (customerKey is not null)
        {
            await DeleteSessionKeyAsync(customerKey);
        }

        // Legacy local keys from earlier builds — wipe so old carts never reappear.
        Changed?.Invoke();
    }

    public async Task EnsureLoadedAsync(CancellationToken cancellationToken = default)
    {
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
                var lines = await ReadSessionLinesAsync(SessionKey);
                if (lines is { Count: > 0 })
                {
                    ApplyLines(lines);
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

    public async Task<bool> AddProductAsync(
        int productId,
        int quantity = 1,
        IReadOnlyList<CartLineOption>? options = null,
        CancellationToken cancellationToken = default)
    {
        if (!await EnsureLoadedReadyAsync(cancellationToken))
        {
            return false;
        }

        var dto = await _catalog.GetByIdAsync(productId, cancellationToken);
        if (dto is null)
        {
            return false;
        }

        var product = StoreProductMapper.ToModel(dto);
        if (!product.HasPrice || product.IsOutOfStock)
        {
            return false;
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
        return true;
    }

    public async Task SetQuantityAsync(string lineId, int quantity, CancellationToken cancellationToken = default)
    {
        if (!await EnsureLoadedReadyAsync(cancellationToken))
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
        if (!await EnsureLoadedReadyAsync(cancellationToken))
        {
            return;
        }

        _lines.RemoveAll(l => l.Id == lineId);
        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        if (!await EnsureLoadedReadyAsync(cancellationToken))
        {
            _lines.Clear();
            Changed?.Invoke();
            return;
        }

        _lines.Clear();
        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    private async Task MergeLinesAsync(IReadOnlyList<CartLine> incoming, CancellationToken cancellationToken)
    {
        if (!await EnsureLoadedReadyAsync(cancellationToken))
        {
            return;
        }

        foreach (var add in incoming)
        {
            var existing = _lines.FirstOrDefault(l => l.Signature == add.Signature);
            if (existing is null)
            {
                add.ImageUrl = ProductImageUrl(add.ProductId);
                if (string.IsNullOrWhiteSpace(add.Id))
                {
                    add.Id = Guid.NewGuid().ToString("N");
                }

                add.Options ??= [];
                _lines.Add(add);
            }
            else
            {
                existing.Quantity += add.Quantity;
            }
        }

        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    private async Task<List<CartLine>?> ReadSessionLinesAsync(string key)
    {
        try
        {
            var lines = await _session.GetAsync<List<CartLine>>(key);
            if (lines is { Count: > 0 })
            {
                return lines;
            }
        }
        catch (InvalidOperationException)
        {
            // Storage not ready.
        }

        return null;
    }

    private async Task DeleteSessionKeyAsync(string key)
    {
        try
        {
            await _session.DeleteAsync(key);
        }
        catch (InvalidOperationException)
        {
            // Ignore.
        }
    }

    private async Task<bool> EnsureLoadedReadyAsync(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            await EnsureLoadedAsync(cancellationToken);
            if (_loaded)
            {
                return true;
            }

            await Task.Delay(40 * (attempt + 1), cancellationToken);
        }

        return _loaded;
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
        try
        {
            if (_lines.Count == 0)
            {
                await _session.DeleteAsync(SessionKey);
            }
            else
            {
                await _session.SetAsync(SessionKey, _lines.ToList());
            }
        }
        catch (InvalidOperationException)
        {
            // Browser storage is not available during static/prerender.
        }
    }
}
