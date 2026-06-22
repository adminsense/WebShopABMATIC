#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[SupplierContacts] (legacy: [Crm].[SupplierConact]).</summary>
public class SupplierContact
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int ContactId { get; set; }
    public bool? IsDefault { get; set; }
    public int? ContactFunctionId { get; set; }
}

