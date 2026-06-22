#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Files].[AzureFileFolders] (legacy: [Bestanden].[AzureFileFolder]).</summary>
public class AzureFileFolder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsForCrm { get; set; }
    public bool IsForOrder { get; set; }
    public bool IsForProject { get; set; }
    public bool IsForProduct { get; set; }
    public bool IsForUser { get; set; }
    public bool IsForGeneralUse { get; set; }
    public decimal SortOrder { get; set; }
}

