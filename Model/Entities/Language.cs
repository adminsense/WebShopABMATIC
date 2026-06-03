#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[Languages] (legacy: [Instellingen].[Taal]).</summary>
public class Language
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public string NameFr { get; set; }
}

