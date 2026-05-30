#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[Supplier] (legacy: [Crm].[Supplier]).</summary>
public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public string? CompanyRegistrationNumber { get; set; }
    public string? VatNumber { get; set; }
    public string? SupplierOrderEmail { get; set; }
    public int LanguageId { get; set; }
    public int GeneralLedgerRevenueAccount { get; set; }
    public bool? IsMainSupplier { get; set; }
    public bool IsInactive { get; set; }
    public bool? IsVerified { get; set; }
    public int? PriceListSortOrder { get; set; }
    public string? PriceListName { get; set; }
    public string? Notes { get; set; }
}

