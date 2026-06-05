#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[ProductManuals] (legacy: [Products].[ProductHandleiding]).</summary>
public class ProductManual
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public int ProductId { get; set; }
    public bool? ShowOnWeb { get; set; }
    public bool? SendAutomatically { get; set; }
    public string Extension { get; set; }
}

