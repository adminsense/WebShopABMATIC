#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[Manufacturer] (legacy: [Crm].[Manufacturer]).</summary>
public class Manufacturer
{
    public int ManufacturerId { get; set; }
    public string Name { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int? CityId { get; set; }
    public string? CompanyRegistrationNumber { get; set; }
    public string? VatNumber { get; set; }
}

