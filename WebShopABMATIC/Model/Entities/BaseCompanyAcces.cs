#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[BaseCompanyAccess] (legacy: [Instellingen].[BaseCompanyAccess]).</summary>
public class BaseCompanyAcces
{
    public int Id { get; set; }
    public int BaseCompanyId { get; set; }
    public int UserId { get; set; }
}

