using FluentAssertions;
using NSubstitute;
using WebShopABMATIC.Application.Ports;
using WebShopABMATIC.Application.Store;
using WebShopABMATIC.Web.Services;

namespace WebShopABMATIC.Tests.Unit.Store;

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

public sealed class StoreCartServiceTests
{
    private readonly IStoreCatalogPort _catalog = Substitute.For<IStoreCatalogPort>();
    private readonly InMemoryStoreCartSessionStore _session = new();

    private StoreCartService CreateSut() => new(_catalog, _session);

    [Fact]
    public async Task AddProduct_rejects_out_of_stock()
    {
        _catalog.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(Product(id: 1, price: 10m, stock: 0));

        var sut = CreateSut();
        var ok = await sut.AddProductAsync(1, 1);

        ok.Should().BeFalse();
        sut.HasLines.Should().BeFalse();
    }

    [Fact]
    public async Task AddProduct_rejects_no_price()
    {
        _catalog.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(Product(id: 2, price: null, stock: 5));

        var sut = CreateSut();
        (await sut.AddProductAsync(2)).Should().BeFalse();
    }

    [Fact]
    public async Task AddProduct_guest_persists_and_counts()
    {
        _catalog.GetByIdAsync(3, Arg.Any<CancellationToken>())
            .Returns(Product(id: 3, price: 12.5m, stock: 10));

        var sut = CreateSut();
        (await sut.AddProductAsync(3, 2)).Should().BeTrue();

        sut.ItemCount.Should().Be(2);
        sut.Subtotal.Should().Be(25m);
        sut.IsCustomerCart.Should().BeFalse();
        _session.Contains("store-cart-v1:guest-session").Should().BeTrue();
    }

    [Fact]
    public async Task BindToCustomer_merges_guest_lines_and_clears_guest_key()
    {
        _catalog.GetByIdAsync(4, Arg.Any<CancellationToken>())
            .Returns(Product(id: 4, price: 5m, stock: 20));

        var sut = CreateSut();
        await sut.AddProductAsync(4, 1);
        await sut.BindToCustomerAsync(42);

        sut.IsCustomerCart.Should().BeTrue();
        sut.CustomerId.Should().Be(42);
        sut.ItemCount.Should().Be(1);
        _session.Contains("store-cart-v1:guest-session").Should().BeFalse();
        _session.Contains("store-cart-v1:c42").Should().BeTrue();
    }

    [Fact]
    public async Task SignOutClear_wipes_memory_and_session()
    {
        _catalog.GetByIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(Product(id: 5, price: 9m, stock: 3));

        var sut = CreateSut();
        await sut.AddProductAsync(5);
        await sut.BindToCustomerAsync(7);
        await sut.SignOutClearAsync();

        sut.HasLines.Should().BeFalse();
        sut.IsCustomerCart.Should().BeFalse();
        _session.Contains("store-cart-v1:guest-session").Should().BeFalse();
        _session.Contains("store-cart-v1:c7").Should().BeFalse();
    }

    [Fact]
    public async Task Remove_and_clear()
    {
        _catalog.GetByIdAsync(6, Arg.Any<CancellationToken>())
            .Returns(Product(id: 6, price: 1m, stock: 5));

        var sut = CreateSut();
        await sut.AddProductAsync(6);
        var lineId = sut.Lines[0].Id;
        await sut.RemoveAsync(lineId);
        sut.HasLines.Should().BeFalse();

        await sut.AddProductAsync(6);
        await sut.ClearAsync();
        sut.HasLines.Should().BeFalse();
    }

    private static StoreProductDto Product(int id, decimal? price, int stock) =>
        new()
        {
            Id = id,
            Name = $"P{id}",
            Description = "",
            ImageUrl = "/img.png",
            Price = price,
            Stock = stock,
            MinQuantity = 0,
            IsNew = false,
            ReferenceCode = "",
            HasOptions = false
        };
}
