#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[AutoNumberings] (legacy: [Instellingen].[AutoNummering]).</summary>
public class AutoNumbering
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public string Prefix { get; set; }
    public string Tag { get; set; }
}

