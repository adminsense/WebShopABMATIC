#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[TemplateType] (legacy: [Instellingen].[TemplateType]).</summary>
public class TemplateType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public string DocumentTypeId { get; set; }
}

