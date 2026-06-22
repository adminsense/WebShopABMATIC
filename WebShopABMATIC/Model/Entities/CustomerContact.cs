#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Customers].[CustomerContacts] (legacy: [Klanten].[KlantContact]).</summary>
public class CustomerContact
{
    public int CustomerContactId { get; set; }
    public int? CustomerId { get; set; }
    public int ContactId { get; set; }
    public string? Notes { get; set; }
    public bool? IsDefaultContact { get; set; }
    public string? JobTitle { get; set; }
    public int? SupplierId { get; set; }
    public int? ManufacturerId { get; set; }
}

