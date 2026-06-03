#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[DocumentTemplates] (legacy: [Instellingen].[Templates]).</summary>
public class DocumentTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool EmailTemplate { get; set; }
    public bool FaxTemplate { get; set; }
    public bool LetterTemplate { get; set; }
    public bool IsDefault { get; set; }
    public int Type { get; set; }
    public int? BaseCompanyId { get; set; }
    public int TaalId { get; set; }
    public long? AzureFileId { get; set; }
    public string Subject { get; set; }
}

