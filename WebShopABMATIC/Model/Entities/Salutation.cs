#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[Salutations] (legacy: [Crm].[Aanspreking]).</summary>
public class Salutation
{
    public int Id { get; set; }
    public string SalutationText { get; set; }
    public bool IsMale { get; set; }
    public bool IsFemale { get; set; }
    public int LanguageId { get; set; }
}

