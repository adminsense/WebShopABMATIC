#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Settings].[StaffUsers] (legacy: [Instellingen].[User]).</summary>
public class StaffUser
{
    public int Id { get; set; }
    public bool Crm { get; set; }
    public bool Offerte { get; set; }
    public bool Bestellingen { get; set; }
    public bool Productie { get; set; }
    public bool Boekhouding { get; set; }
    public bool Planning { get; set; }
    public bool Admin { get; set; }
    public bool Verkoper { get; set; }
    public int Color { get; set; }
    public string? ExchangeLastWatermark { get; set; }
    public bool DoSyncExchange { get; set; }
    public bool RechtstreeksGsmNrtonen { get; set; }
    public bool RechtstreeksTelefoonNrTonen { get; set; }
    public string? LinkedInUrl { get; set; }
    public int? UserGroupId { get; set; }
    public bool DirecteLosseVerkoop { get; set; }
    public bool? ReportSales { get; set; }
    public string? DefaultCeLabelPrinter { get; set; }
    public string? DefaultProductionLabelPrinter { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string? EmailTemplate { get; set; }
    public string? Tel { get; set; }
    public string? FaxTemplate { get; set; }
    public string? Gsm { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int BaseCompaniesId { get; set; }
    public int TaalId { get; set; }
    public string? Ice1Number { get; set; }
    public string? Ice1Name { get; set; }
    public string? Ice2Number { get; set; }
    public string? Ice2Name { get; set; }
    public string? PrivateEmail { get; set; }
    public bool? Hr { get; set; }
    public string Address { get; set; }
    public DateTime HiredAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public bool ProductBeheer { get; set; }
    public bool PlanningBinnenDienst { get; set; }
    public bool PlanningBuitendienst { get; set; }
    public bool PlanningProjecten { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyMobile { get; set; }
    public string? JobTitle { get; set; }
    public bool? ToonInPrijslijst { get; set; }
    public bool? SelectForInternalPlanning { get; set; }
    public bool? SelectForExternalPlanning { get; set; }
    public bool? SelectForProjectPlanning { get; set; }
    public bool? SelectForLeavePlanning { get; set; }
    public int? DefaultPlanningLabelId { get; set; }
    public int TextColor { get; set; }
    public bool? CanAccessRevenueReports { get; set; }
    public bool? CanAccessProfitReports { get; set; }
    public bool? CanAccessDmsSpecial { get; set; }
    public bool? CanAccessBulkOrders { get; set; }
    public bool? CanAccessStockManagement { get; set; }
    public bool? CanOrderFromOrderScreen { get; set; }
    public bool? ShowProjects { get; set; }
    public bool? CanAccessBilling { get; set; }
}

