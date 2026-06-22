#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Projects].[ProjectPartyContacts] (legacy: [Projecten].[ProjectPartijContact]).</summary>
public class ProjectPartyContact
{
    public int Id { get; set; }
    public int CustomerContactId { get; set; }
    public int ProjectPartijId { get; set; }
    public bool EmailQuote { get; set; }
    public bool EmailOrderConfirmation { get; set; }
    public bool EmailPlanning { get; set; }
    public bool EmailDeliveryReady { get; set; }
    public bool EmailDelivered { get; set; }
    public bool EmailBilling { get; set; }
    public string Note { get; set; }
}

