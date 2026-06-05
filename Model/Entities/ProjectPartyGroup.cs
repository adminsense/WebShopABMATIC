#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[ProjectPartyGroups] (legacy: [Projecten].[ProjectPartijGroep]).</summary>
public class ProjectPartyGroup
{
    public int Id { get; set; }
    public string NameEn { get; set; }
    public string NameFr { get; set; }
    public string NameNl { get; set; }
    public bool IsForSupplier { get; set; }
    public bool IsForCustomer { get; set; }
    public bool IsForManufacturer { get; set; }
}

