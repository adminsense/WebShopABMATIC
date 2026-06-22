#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerJobCodeRates] (legacy: [Crm].[KlantJobcodeTarief]).</summary>
public class CustomerJobCodeRate
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int JobCodeId { get; set; }
    public decimal HourlyRate { get; set; }
}

