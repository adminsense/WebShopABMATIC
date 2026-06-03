#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerStatuses] (legacy: [Crm].[KlantStatus]).</summary>
public class CustomerStatus
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public bool? IsDefault { get; set; }
}

