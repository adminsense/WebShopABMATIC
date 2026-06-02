#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[ContactProjectRoles] (legacy: [Crm].[ContactProjectRol]).</summary>
public class ContactProjectRole
{
    public int Id { get; set; }
    public string NameNl { get; set; }
    public string NameFr { get; set; }
    public string NameEn { get; set; }
    public bool DeactivateAfterOrderClosed { get; set; }
}

