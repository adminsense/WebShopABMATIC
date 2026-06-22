#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[AppSettings] (legacy: [Instellingen].[Parameter]).</summary>
public class AppSetting
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public int? BaseCompanyId { get; set; }
}

