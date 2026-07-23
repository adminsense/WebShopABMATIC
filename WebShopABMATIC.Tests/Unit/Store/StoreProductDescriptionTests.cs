using FluentAssertions;
using WebShopABMATIC.Application.Store;

namespace WebShopABMATIC.Tests.Unit.Store;

public sealed class StoreProductDescriptionTests
{
    [Fact]
    public void Resolve_prefers_webshop_nl()
    {
        StoreProductDescription.Resolve("Web NL", "NL", "EN", "FR")
            .Should().Be("Web NL");
    }

    [Fact]
    public void Resolve_falls_back_through_nl_en_fr()
    {
        StoreProductDescription.Resolve(null, "  NL  ", "EN", "FR").Should().Be("NL");
        StoreProductDescription.Resolve(" ", null, "EN", "FR").Should().Be("EN");
        StoreProductDescription.Resolve(null, null, null, "FR").Should().Be("FR");
    }

    [Fact]
    public void Resolve_returns_empty_when_all_missing()
    {
        StoreProductDescription.Resolve(null, " ", "", null).Should().BeEmpty();
    }
}
