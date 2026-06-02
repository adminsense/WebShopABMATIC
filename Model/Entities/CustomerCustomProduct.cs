#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerCustomProducts] (legacy: [Crm].[KlantMaatProduct]).</summary>
public class CustomerCustomProduct
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductEenheidId { get; set; }
    public string? ArticleNumber { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public string? SupplierArticleNumber { get; set; }
}

