#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[JobCode] (legacy: [Projecten].[JobCode]).</summary>
public class JobCode
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string NameNl { get; set; }
    public string NameFr { get; set; }
    public string NameEn { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsBillable { get; set; }
}

