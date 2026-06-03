#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Products].[IntrastatCode] (legacy: [Products].[IntrastatCode]).</summary>
public class IntrastatCode
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    public string? MainGroup { get; set; }
    public string? SubGroup { get; set; }
}

