#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerDeliveryAddresses] (legacy: [Crm].[KlantLeveradres]).</summary>
public class CustomerDeliveryAddress
{
    public int Id { get; set; }
    public string Straat { get; set; }
    public string Number { get; set; }
    public string Bus { get; set; }
    public int CityId { get; set; }
    public string Notes { get; set; }
    public int? ContactId { get; set; }
    public int? CustomerId { get; set; }
    public string Name { get; set; }
}

