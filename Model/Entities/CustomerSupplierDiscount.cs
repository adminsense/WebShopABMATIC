#nullable enable
using System;

namespace WebShopABMATIC.Data.Entities;

/// <summary>Entity for [Crm].[CustomerSupplierDiscounts] (legacy: [Crm].[KlantLeverancierKorting]).</summary>
public class CustomerSupplierDiscount
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal DiscountPercentage { get; set; }
    public int SupplierId { get; set; }
    public string? Notes { get; set; }
    public DateTime FromAddress { get; set; }
    public DateTime? ValidTo { get; set; }
    public int? CustomerTypeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public int? InstallationCustomerTypeId { get; set; }
    public decimal? InstallationDiscountPercentage { get; set; }
}

