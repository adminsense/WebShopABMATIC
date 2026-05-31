#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Customers].[Contact] (legacy: [Klanten].[Contact]).</summary>
public class Contact
{
    public int ContactId { get; set; }
    public string ContactBox { get; set; }
    public string ContactEmail { get; set; }
    public string ContactFax { get; set; }
    public string ContactHouseNumber { get; set; }
    public string ContactLogin { get; set; }
    public string ContactMobile { get; set; }
    public string ContactLastName { get; set; }
    public string ContactPassword { get; set; }
    public string ContactStreet { get; set; }
    public string ContactPhone { get; set; }
    public string ContactFirstName { get; set; }
    public bool IsInstallerContact { get; set; }
    public int ContactCityId { get; set; }
    public bool IsInternalUserContact { get; set; }
    public int? SalutationId { get; set; }
    public int ContactLanguageId { get; set; }
    public int? BaseCompanyId { get; set; }
    public string? InstallerDisplayName { get; set; }
    public string? ContactJobTitle { get; set; }
    public bool EmailQuote { get; set; }
    public bool EmailOrderConfirmation { get; set; }
    public bool EmailPlanning { get; set; }
    public bool EmailDeliveryReady { get; set; }
    public bool EmailDelivered { get; set; }
    public bool EmailBilling { get; set; }
    public DateTime? LeftAt { get; set; }
    public string? ContactBuilding { get; set; }
}

