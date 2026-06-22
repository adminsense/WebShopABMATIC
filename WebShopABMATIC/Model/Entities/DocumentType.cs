#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Accounting].[DocumentTypes] (legacy: [Boekhouding].[DocumentType]).</summary>
public class DocumentType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameFr { get; set; }
    public int ParameterId { get; set; }
    public string NameEn { get; set; }
}

