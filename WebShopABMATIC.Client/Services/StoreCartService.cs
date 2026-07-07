using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Web.Services;

public sealed class StoreCartService
{
    private const string StorageKey = "store-cart-v1";

    private readonly IStoreCatalogPort _catalog;
    private readonly ProtectedLocalStorage _storage;
    private readonly List<CartLine> _lines = [];
    private readonly SemaphoreSlim _loadGate = new(1, 1);
    private bool _loaded;

    public StoreCartService(IStoreCatalogPort catalog, ProtectedLocalStorage storage)
    {
        _catalog = catalog;
        _storage = storage;
    }

    public event Action? Changed;

    public IReadOnlyList<CartLine> Lines => _lines;

    public int ItemCount => _lines.Sum(l => l.Quantity);

    public decimal Subtotal => _lines.Sum(l => l.LineTotal);

    public decimal DeliveryFee => _lines.Count > 0 ? 9.00m : 0m;

    public decimal VatAmount => Math.Round((Subtotal + DeliveryFee) * 0.21m, 2);

    public decimal Total => Subtotal + DeliveryFee + VatAmount;

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
                var result = await _storage.GetAsync<List<CartLine>>(StorageKey);
                if (result.Success && result.Value is { Count: > 0 })
                {
                    _lines.Clear();
                    _lines.AddRange(result.Value);
                }
            }
            catch (InvalidOperationException)
            {
                // Browser storage is not available during static/prerender.
            }

            _loaded = true;
        }
        finally
        {
            _loadGate.Release();
        }
    }

    public async Task AddProductAsync(int productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);

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

        var line = _lines.FirstOrDefault(l => l.ProductId == productId);
        if (line is null)
        {
            _lines.Add(new CartLine
            {
                ProductId = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                UnitPrice = product.Price!.Value,
                Quantity = quantity
            });
        }
        else
        {
            line.Quantity = Math.Min(line.Quantity + quantity, Math.Max(product.Stock, 1));
        }

        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    public async Task SetQuantityAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);

        var line = _lines.FirstOrDefault(l => l.ProductId == productId);
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

    public async Task RemoveAsync(int productId, CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);

        _lines.RemoveAll(l => l.ProductId == productId);
        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await EnsureLoadedAsync(cancellationToken);

        _lines.Clear();
        await PersistAsync(cancellationToken);
        Changed?.Invoke();
    }

    private async Task PersistAsync(CancellationToken cancellationToken)
    {
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
