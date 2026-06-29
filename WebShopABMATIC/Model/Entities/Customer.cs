#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Customers].[Customers] (legacy: [Klanten].[Klant]).</summary>
public class Customer
{
    public int CustomerId { get; set; }
    public string CustomerVatNumber { get; set; }
    public string CustomerBox { get; set; }
    public string CustomerHouseNumber { get; set; }
    public string CustomerName { get; set; }
    public string CustomerStreet { get; set; }
    public int CustomerTypeId { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerFax { get; set; }
    public string? CustomerNotes { get; set; }
    public string CustomerEmail { get; set; }
    public int CustomerVatSystemId { get; set; }
    public int CustomerStatusId { get; set; }
    public int CustomerCityId { get; set; }
    public int? CustomerActivityId { get; set; }
    public int? CustomerNumber { get; set; }
    public int AccountManagerUserId { get; set; }
    public string? FirstContactName { get; set; }
    public DateTime? LockedTime { get; set; }
    public bool Locked { get; set; }
    public string LockedBy { get; set; }
    public decimal RevenueLast12Months { get; set; }
    public bool SendPriceListByEmail { get; set; }
    public bool PromotionalMailing { get; set; }
    public int DeliveryTypeId { get; set; }
    public bool InvoicesByMail { get; set; }
    public string? CustomerInternalNotes { get; set; }
    public bool DigitalInvoicing { get; set; }
    public string? CElabelName { get; set; }
    public string? CElabelNr { get; set; }
    public int? CustomerPaymentStatus { get; set; }
    public int? BaseCompaniesId { get; set; }
    public bool? IsInternalCompany { get; set; }
    public int CustomerLanguageId { get; set; }
    public bool? PriceListResContractor { get; set; }
    public bool? PriceListResDealer { get; set; }
    public bool? PriceListResConsumer { get; set; }
    public bool? PriceListIndContractor { get; set; }
    public bool? PriceListIndDealer { get; set; }
    public string? CEemail { get; set; }
    public byte[]? Logo { get; set; }
    public string CustomerGroup { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool? IsVerified { get; set; }
    public int QuoteContactId { get; set; }
    public int OrderConfirmationContactId { get; set; }
    public int PlanningContactId { get; set; }
    public int DeliveryCompleteContactId { get; set; }
    public int BillingContactId { get; set; }
    public int? CommissionRecipientId { get; set; }
    public decimal RequestedCommission { get; set; }
    public int BetaaltermijnId { get; set; }
    public string? WebshopLogin { get; set; }
    public string? WebshopPasswordHash { get; set; }
    public string? WebshopPasswordSalt { get; set; }
    public string? CustomerBuildingName { get; set; }
    public DateTime? LaatsteFollowUp { get; set; }
    public int DeliveryCustomerTypeId { get; set; }
    public string? PeppolIdSchema { get; set; }
    public string? PeppolId { get; set; }
}

