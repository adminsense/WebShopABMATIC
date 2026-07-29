#nullable enable

namespace WebShopABMATIC.Data.Entities;

/// <summary>
/// Entity for [Products].[ProductAttribuutItem] — per-product attribute value (Waarde).
/// DE-PARA: ProductAttributeId←ProductAttribuutId, ProductId←ProductProdId→Product.ProdId, Value←Waarde.
/// </summary>
public class ProductAttributeValue
{
    public int Id { get; set; }
    public int ProductAttributeId { get; set; }
    public int ProductId { get; set; }
    public string Value { get; set; } = string.Empty;
}
