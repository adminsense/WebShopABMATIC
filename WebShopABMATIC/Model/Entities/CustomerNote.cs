#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerNotes] (legacy: [Crm].[KlantOpmerkingen]).</summary>
public class CustomerNote
{
    public int CustomerId { get; set; }
    public int? DocumentTypeId { get; set; }
    public string Notes { get; set; }
    public int Id { get; set; }
}

