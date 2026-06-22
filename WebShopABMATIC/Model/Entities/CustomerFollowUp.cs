#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerFollowUps] (legacy: [Crm].[KlantFollowUp]).</summary>
public class CustomerFollowUp
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
}

