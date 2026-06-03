#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Emails].[EmailQueues] (legacy: [Emails].[EmailQueue]).</summary>
public class EmailQueue
{
    public int Id { get; set; }
    public string Name { get; set; }
}

