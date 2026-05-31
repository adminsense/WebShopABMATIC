#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[ProjectContacts] (legacy: [Crm].[ProjectContact]).</summary>
public class ProjectContact
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int? OrderId { get; set; }
    public int ContactProjectRolId { get; set; }
    public int LinkedContactId { get; set; }
    public int ContactContactId { get; set; }
}

