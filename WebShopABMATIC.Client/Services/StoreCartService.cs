using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Web.Services;

public sealed class StoreCartService
{
    private const string SessionKey = "store-cart-v1";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly IStoreCatalogPort _catalog;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly List<CartLine> _lines = [];

    public StoreCartService(IStoreCatalogPort catalog, IHttpContextAccessor httpContextAccessor)
    {
        _catalog = catalog;
        _httpContextAccessor = httpContextAccessor;
        LoadFromSession();
    }

    public event Action? Changed;

    public IReadOnlyList<CartLine> Lines => _lines;

    public int ItemCount => _lines.Sum(l => l.Quantity);

    public decimal Subtotal => _lines.Sum(l => l.LineTotal);

    public decimal DeliveryFee => _lines.Count > 0 ? 9.00m : 0m;

    public decimal VatAmount => Math.Round((Subtotal + DeliveryFee) * 0.21m, 2);

    public decimal Total => Subtotal + DeliveryFee + VatAmount;

    public async Task AddProductAsync(int productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
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

        Persist();
        Changed?.Invoke();
    }

    public void SetQuantity(int productId, int quantity)
    {
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

        Persist();
        Changed?.Invoke();
    }

    public void Remove(int productId)
    {
        _lines.RemoveAll(l => l.ProductId == productId);
        Persist();
        Changed?.Invoke();
    }

    public void Clear()
    {
        _lines.Clear();
        Persist();
        Changed?.Invoke();
    }

    private void LoadFromSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session is null || !session.TryGetValue(SessionKey, out var bytes) || bytes.Length == 0)
        {
            return;
        }

        try
        {
            var lines = JsonSerializer.Deserialize<List<CartLine>>(bytes, JsonOptions);
            if (lines is { Count: > 0 })
            {
                _lines.Clear();
                _lines.AddRange(lines);
            }
        }
        catch (JsonException)
        {
            session.Remove(SessionKey);
        }
    }

    private void Persist()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session is null)
        {
            return;
        }

        if (_lines.Count == 0)
        {
            session.Remove(SessionKey);
            return;
        }

        session.Set(SessionKey, JsonSerializer.SerializeToUtf8Bytes(_lines, JsonOptions));
    }
}
