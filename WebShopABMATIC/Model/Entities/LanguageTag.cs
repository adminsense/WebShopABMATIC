#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[LanguageTags] (legacy: [Instellingen].[LangTag]).</summary>
public class LanguageTag
{
    public int Id { get; set; }
    public string FieldName { get; set; }
    public string NL { get; set; }
    public string FR { get; set; }
    public string SourceKey { get; set; }
    public string En { get; set; }
}

