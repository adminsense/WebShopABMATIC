#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[UserGroups] (legacy: [Instellingen].[UsrGroep]).</summary>
public class UserGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsInstallationTeam { get; set; }
    public bool IsServiceTeam { get; set; }
    public bool IsTransportTeam { get; set; }
    public int? OrderStatusGroupId { get; set; }
}

