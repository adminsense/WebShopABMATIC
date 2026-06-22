using WebShopABMATIC.Application.Ports;

namespace WebShopABMATIC.Web.Services;

public sealed class StoreCartService
{
    private readonly IStoreCatalogPort _catalog;
    private readonly List<CartLine> _lines = [];

    public StoreCartService(IStoreCatalogPort catalog) => _catalog = catalog;

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
            line.Quantity = Math.Min(line.Quantity + quantity, product.Stock);
        }

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

        Changed?.Invoke();
    }

    public void Remove(int productId)
    {
        _lines.RemoveAll(l => l.ProductId == productId);
        Changed?.Invoke();
    }

    public void Clear()
    {
        _lines.Clear();
        Changed?.Invoke();
    }
}
