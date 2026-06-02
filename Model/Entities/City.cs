#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[City] (legacy: [Crm].[City]).</summary>
public class City
{
    public int CityId { get; set; }
    public string CityName { get; set; }
    public string PostalCode { get; set; }
    public string CountryName { get; set; }
    public string? CountryIsoCode { get; set; }
    public int? CountryId { get; set; }
}

